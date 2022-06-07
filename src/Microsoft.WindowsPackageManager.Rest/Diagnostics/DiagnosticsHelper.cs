// -----------------------------------------------------------------------
// <copyright file="DiagnosticsHelper.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WindowsPackageManager.Rest.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.Extensions.Logging;
    using Microsoft.Msix.Utils.Logger;
    using Microsoft.WindowsPackageManager.Rest.Diagnostics;

    /// <summary>
    /// A singleton class containing properties and helpers related to diagnostics and logging.
    /// </summary>
    public class DiagnosticsHelper
    {
        private const string CommandTag = "Command";
        private const string AzureFunctionNameTag = "FunctionName";
        private const string AzureFunctionInvocationIdTag = "InvocationId";
        private const string OperationIdTag = "OperationId";
        private const string ManifestIdTag = "ManifestId";
        private const string InstallerIdTag = "InstallerId";

        /// <summary>
        /// Factory for singleton instance creation.
        /// </summary>
        private static Lazy<DiagnosticsHelper> sfactory = new Lazy<DiagnosticsHelper>(() => new DiagnosticsHelper());

        /// <summary>
        /// Gets the singleton instance of DiagnosticsHelper.
        /// </summary>
        public static DiagnosticsHelper Instance
        {
            get { return sfactory.Value; }
        }

        /// <summary>
        /// Sets up logging.
        /// </summary>
        /// <param name="logger">Logger from Microsoft.Extensions.Logging.</param>
        /// <param name="verboseConsoleLogging">Whether to print verbose logs to console. True by default.</param>
        /// <param name="setupGenevaTelemetry">Whether to setup geneva telemetry. True by default.</param>
        /// <param name="setupConsoleLogger">Whether to setup console logger.</param>
        /// <param name="monitorRole">variable for role for setup geneva telemetry.</param>
        /// <param name="monitorTenant">variable for tenant for setup geneva telemetry.</param>
        /// <param name="appServiceEnvironment">variable for app service environment for setup geneva telemetry.</param>
        public void SetupAzureFunctionLoggerAndGenevaTelemetry(
            ILogger logger,
            bool verboseConsoleLogging = true,
            bool setupGenevaTelemetry = true,
            bool setupConsoleLogger = true,
            string monitorRole = "",
            string monitorTenant = "",
            string appServiceEnvironment = "")
        {
            Logger.LogLevels consoleLogLevel = verboseConsoleLogging ?
                Logger.LogLevels.All : Logger.LogLevels.All & ~Logger.LogLevels.Debug;

            if (setupConsoleLogger)
            {
                LogUtils.SetupConsoleLogger();
            }

            // Setup ILogger from Microsoft.Extensions.Logging.
            AzureFunctionLogger azureFunctionLogger = new AzureFunctionLogger(logger)
            {
                LogLevels = consoleLogLevel,
                LogDecorations = Logger.LogDecorations.All,
            };

            // Register the logger wrapper that uses ILogger from Microsoft.Extensions.Logging.
            Logger.AddLogProvider(azureFunctionLogger);

            if (setupGenevaTelemetry)
            {
                // Initialize Ifx tracing for logging to Geneva Monitoring
                TelemetryOperation.Initialize(
                    monitorRole,
                    monitorTenant,
                    appServiceEnvironment);

                // Setup Ifx trace logger.
                AppServiceGenevaTraceLogger packageManagerGenevaTraceLogger = new AppServiceGenevaTraceLogger()
                {
                    LogLevels = consoleLogLevel,
                    LogDecorations = Logger.LogDecorations.All,
                };

                // Register the logger wrapper that logs to Geneva.
                Logger.AddLogProvider(packageManagerGenevaTraceLogger);
            }
        }

        /// <summary>
        /// Sets up file logger.
        /// </summary>
        /// <param name="logFilePath">Log file path.</param>
        public void SetupFileLogger(string logFilePath)
        {
            LogUtils.SetupFileLogger(logFilePath);
        }

        /// <summary>
        /// Method to create the logging context.
        /// </summary>
        /// <param name="command">Command.</param>
        /// <param name="operationId">Operation id.</param>
        /// <returns>A LoggingContext with context information.</returns>
        public LoggingContext GetLoggingContextForCommand(string command, string operationId)
        {
            return new LoggingContext(new Dictionary<string, string>()
            {
                { CommandTag, command },
                { OperationIdTag, operationId },
            });
        }

        /// <summary>
        /// Method to create the logging context for azure functions.
        /// </summary>
        /// <param name="functionName">Azure function name.</param>
        /// <param name="invocationId">Azure function invocation id.</param>
        /// <param name="operationId">Operation id.</param>
        /// <param name="manifestId">Optional Manifest id tag.</param>
        /// <param name="installerId">Optional Installer id tag.</param>
        /// <returns>A LoggingContext with context information.</returns>
        public LoggingContext GetLoggingContext(
            string functionName,
            string invocationId,
            string operationId,
            string manifestId = null,
            string installerId = null)
        {
            var tags = new Dictionary<string, string>()
            {
                { AzureFunctionNameTag, functionName },
                { AzureFunctionInvocationIdTag, invocationId },
                { OperationIdTag, operationId },
            };

            if (!string.IsNullOrEmpty(manifestId))
            {
                tags.Add(ManifestIdTag, manifestId);
            }

            if (!string.IsNullOrEmpty(installerId))
            {
                tags.Add(InstallerIdTag, installerId);
            }

            return new LoggingContext(tags);
        }
    }
}
