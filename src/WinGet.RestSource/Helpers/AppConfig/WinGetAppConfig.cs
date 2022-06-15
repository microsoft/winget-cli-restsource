// -----------------------------------------------------------------------
// <copyright file="WinGetAppConfig.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Helpers.AppConfig
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using global::Azure.Data.AppConfiguration;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Configuration.AzureAppConfiguration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.FeatureManagement;
    using Microsoft.Msix.Utils.Logger;
    using Microsoft.WindowsPackageManager.Rest.Diagnostics;

    /// <summary>
    ///     WinGet App Configuration.
    /// </summary>
    public class WinGetAppConfig : IWinGetAppConfig
    {
        private const string EnabledFalse = "\"enabled\":false";
        private const string EnabledTrue = "\"enabled\":true";

        private static readonly Lazy<WinGetAppConfig> LazyInstance = new Lazy<WinGetAppConfig>(() => new WinGetAppConfig());

        private readonly string appConfigurationPrimary = Environment.GetEnvironmentVariable("WinGetRest:AppConfig:Primary");
        private readonly string appConfigurationSecondary = Environment.GetEnvironmentVariable("WinGetRest:AppConfig:Secondary");

        private readonly Dictionary<FeatureFlag, bool> defaultValue = new Dictionary<FeatureFlag, bool>();
        private readonly string failedReason;
        private readonly IFeatureManager featureManager;

        private bool isLoaded;
        private IConfigurationRefresher refresher;

        private WinGetAppConfig()
        {
            if (!string.IsNullOrWhiteSpace(this.appConfigurationPrimary) ||
                !string.IsNullOrWhiteSpace(this.appConfigurationSecondary))
            {
                try
                {
                    // https://docs.microsoft.com/en-us/azure/azure-app-configuration/concept-disaster-recovery
                    // Currently, Azure App Configuration is a regional service. App Configuration doesn't offer
                    // automatic failover to another region. To support this we have a primary and secondary
                    // Azure App Configuration. Technically, this is not executing a failover. It's attempting to
                    // retrieve the same set of configuration data from two App Configuration stores simultaneously.
                    // To enable this load from the secondary store first and then the primary store. This approach
                    // ensures that the configuration data in the primary store takes precedence whenever it's available.
                    // Passing true as the second parameter of AddAzureAppConfiguration makes it not fail.
                    var builder = new ConfigurationBuilder();

                    // Secondary
                    if (!string.IsNullOrWhiteSpace(this.appConfigurationSecondary))
                    {
                        builder.AddAzureAppConfiguration(
                            options =>
                            {
                                this.Load(options, this.appConfigurationSecondary);
                            },
                            true);
                    }

                    // Primary
                    if (!string.IsNullOrWhiteSpace(this.appConfigurationPrimary))
                    {
                        builder.AddAzureAppConfiguration(
                            options =>
                            {
                                this.Load(options, this.appConfigurationPrimary);
                            },
                            true);
                    }

                    IConfiguration configuration = builder.Build();

                    if (this.isLoaded)
                    {
                        var services = new ServiceCollection();
                        services.AddSingleton(configuration).AddFeatureManagement();
                        this.featureManager = services.BuildServiceProvider().GetRequiredService<IFeatureManager>();
                    }
                    else
                    {
                        this.failedReason = "Failed to load AddAzureAppConfiguration";
                    }
                }
                catch (Exception e)
                {
                    // Can't pass the logging context here, so if the first time we see if a feature
                    // is enabled we will get this info.
                    this.failedReason = e.ToString();
                }
            }
            else
            {
                this.failedReason = "Connection string is not set.";
            }

            // Add default values
            this.defaultValue[FeatureFlag.GenevaLogging] = true;
        }

        /// <summary>
        ///     Gets instance.
        /// </summary>
        public static WinGetAppConfig Instance => LazyInstance.Value;

        /// <summary>
        ///     Modifies a feature flag using the specified Azure App Config connection string at runtime.
        ///     WARNING: If the WinGetAppConfig is already initialized the change will be reflected in 30s because that's
        ///     the refresh timeout.
        /// </summary>
        /// <param name="flag">Feature flag.</param>
        /// <param name="value">New value.</param>
        /// <param name="connectionString">Read-Write connection string.</param>
        /// <param name="loggingContext">Logging Context.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public static async Task ModifyFeatureFlagAsync(FeatureFlag flag, bool value, string connectionString, LoggingContext loggingContext)
        {
            // A value of a feature flag in Azure App Config is not just true or false. It looks something like
            // .appconfig.featureflag/Feature,{"id":"Feature","description":"","enabled":false,"conditions":{"client_filters":[]}}
            // And I couldn't find a way to modify a feature flag as it only supports modifying configuration.
            // A feature flag is really just a configuration that starts with ".appconfig.featureflag/" has this different
            // json value and a different content type. So the simplest way is just to do a string replace in the value obtained
            // with what we want.
            var client = new ConfigurationClient(connectionString);
            var setting = await client.GetConfigurationSettingAsync($".appconfig.featureflag/{flag}");

            string newValue;
            if (value)
            {
                newValue = setting.Value.Value.Replace(EnabledFalse, EnabledTrue);
            }
            else
            {
                newValue = setting.Value.Value.Replace(EnabledTrue, EnabledFalse);
            }

            Logger.Info($"{loggingContext}Modifying Feature flag {flag}. Old value '{setting.Value.Value}' New value '{newValue}'");
            setting.Value.Value = newValue;
            _ = await client.SetConfigurationSettingAsync(setting);
        }

        /// <inheritdoc />
        public async Task<bool> IsEnabledAsync(FeatureFlag flag, LoggingContext loggingContext)
        {
            bool result;
            if (!this.isLoaded)
            {
                Logger.Warning($"{loggingContext}AppConfiguration failed initializing. {this.failedReason}. Using default value.");
                result = this.defaultValue[flag];
            }
            else
            {
                try
                {
                    // Default refresh time is 30 seconds.
                    bool retryResult = await this.RunWithRetryAsync(async () => { return await this.refresher.TryRefreshAsync(); });
                    if (!retryResult)
                    {
                        Logger.Warning($"{loggingContext}Failed refreshing app config.");
                    }

                    result = await this.featureManager.IsEnabledAsync(flag.ToString());
                }
                catch (Exception e)
                {
                    Logger.Warning($"{loggingContext}Error retrieving value for {flag}. '{e}' Using default value.");
                    result = this.defaultValue[flag];
                }
            }

            Logger.Info($"{loggingContext}Feature {flag} is {(result ? "enabled" : "disabled")}");
            return result;
        }

        /// <inheritdoc />
        public bool IsEnabled(FeatureFlag flag, LoggingContext loggingContext)
        {
            return this.IsEnabledAsync(flag, loggingContext).Result;
        }

        private void Load(AzureAppConfigurationOptions options, string connectionString)
        {
            try
            {
                options.Connect(connectionString)
                    .UseFeatureFlags()
                    .ConfigureRefresh(refresh =>
                    {
                        foreach (FeatureFlag flag in Enum.GetValues(typeof(FeatureFlag)))
                        {
                            refresh.Register($".appconfig.featureflag/{flag}", true);
                        }
                    });
                this.refresher = options.GetRefresher();
                this.isLoaded = true;
            }
            catch (ArgumentNullException)
            {
                // If the connection string is null we will fail loading. Catch this exception so if the other connection
                // string is not null it gets loaded. We don't have logger here.
            }
        }

        /// <summary>
        ///     Retry when the function returns a specific value. Copy paste from RetryHelper because don't want
        ///     to add dependencies to this project.
        /// </summary>
        /// <param name="func">Function to call.</param>
        /// <returns>TReturn. If the retries expired, then return the retry value.</returns>
        private async Task<bool> RunWithRetryAsync(Func<Task<bool>> func)
        {
            const int maxRetries = 3;
            const int waitTime = 10000;

            int tries = 0;
            while (true)
            {
                bool returnValue = await func();

                if (returnValue)
                {
                    return returnValue;
                }

                if (++tries < maxRetries)
                {
                    await Task.Delay(waitTime);
                }
                else
                {
                    return returnValue;
                }
            }
        }
    }
}
