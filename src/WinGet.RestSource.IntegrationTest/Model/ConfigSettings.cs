// -----------------------------------------------------------------------
// <copyright file="ConfigSettings.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.IntegrationTest.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Xunit.Sdk;

    /// <summary>
    /// Config settings data model.
    /// </summary>
    public class ConfigSettings
    {
        private ConfigSettings(
            string restSourceUrl,
            string functionsHostKey)
        {
            this.RestSourceUrl = restSourceUrl;
            this.FunctionsHostKey = functionsHostKey;
        }

        /// <summary>
        /// Gets the rest source url.
        /// </summary>
        public string RestSourceUrl { get; }

        /// <summary>
        /// Gets the function host key.
        /// </summary>
        public string FunctionsHostKey { get; }

        /// <summary>
        /// Build the config settings data model.
        /// </summary>
        /// <param name="settingJsonFile">Settings json file.</param>
        /// <returns>An object of type <see cref="ConfigSettings"/>.</returns>
        /// <exception cref="NullException">Throws null exception if settings or environment variable not found.</exception>
        public static ConfigSettings Build(string settingJsonFile = "Test.runsettings.json")
        {
            var configuration = new ConfigurationBuilder()

                // Defaults specified in the Test.runsettings.json
                .AddJsonFile(settingJsonFile, true)

                // But they can be overridden using environment variables
                .AddEnvironmentVariables()
                .Build();

            const string restSourceUrlKey = "RestSourceUrl";
            const string functionsHostKeyKey = "FunctionsHostKey";

            string restSourceUrl = configuration[restSourceUrlKey] ?? throw new NullException(restSourceUrlKey);
            restSourceUrl = restSourceUrl.TrimEnd('/');

            string functionHostKey = configuration[functionsHostKeyKey] ?? throw new NullException(functionsHostKeyKey);

            return new ConfigSettings(restSourceUrl, functionHostKey);
        }
    }
}
