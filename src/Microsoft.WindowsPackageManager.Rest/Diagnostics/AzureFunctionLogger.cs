// -----------------------------------------------------------------------
// <copyright file="AzureFunctionLogger.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.OWCUtils.Diagnostics
{
    using System;
    using Microsoft.Extensions.Logging;
    using Microsoft.Msix.Utils.Logger;

    /// <summary>
    /// Log provider wrapper that logs to Azure App Insights using Azure function ILogger.
    /// </summary>
    public class AzureFunctionLogger : LogProvider
    {
        private readonly ILogger azureFunctionLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureFunctionLogger"/> class.
        /// </summary>
        /// <param name="logger">Microsoft extensions ILogger object.</param>
        public AzureFunctionLogger(ILogger logger)
        {
            this.azureFunctionLogger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Log a message.
        /// </summary>
        /// <param name="logMessage">log message.</param>
        public override void Log(ILogMessage logMessage)
        {
            _ = logMessage ?? throw new ArgumentNullException(nameof(logMessage));

            // Output the message.
            switch (logMessage.LogLevel)
            {
                case Logger.LogLevels.Critical:
                    this.azureFunctionLogger.LogCritical(logMessage.GetLogMessage(this.LogDecorations, this.LogLevels));
                    break;

                case Logger.LogLevels.Error:
                    this.azureFunctionLogger.LogError(logMessage.GetLogMessage(this.LogDecorations, this.LogLevels));
                    break;

                case Logger.LogLevels.InfoPrompt:
                    this.azureFunctionLogger.LogInformation(logMessage.GetLogMessage(this.LogDecorations, this.LogLevels));
                    break;

                case Logger.LogLevels.Warning:
                    this.azureFunctionLogger.LogWarning(logMessage.GetLogMessage(this.LogDecorations, this.LogLevels));
                    break;

                default:
                    this.azureFunctionLogger.LogInformation(logMessage.GetLogMessage(this.LogDecorations, this.LogLevels));
                    break;
            }
        }

        /// <summary>
        /// Deinitialize logger.
        /// </summary>
        public override void DeinitLog()
        {
        }
    }
}
