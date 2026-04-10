// -----------------------------------------------------------------------
// <copyright file="GenevaLogger.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WindowsPackageManager.Rest.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.Extensions.Logging;
    using Microsoft.Msix.Utils.Logger;

    /// <summary>
    /// Log provider wrapper that logs to Geneva.
    /// </summary>
    public class GenevaLogger : LogProvider
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenevaLogger"/> class.
        /// </summary>
        public GenevaLogger()
        {
            var loggerFactory = LoggerFactory.Create(builder => builder
            .AddOpenTelemetry(loggerOptions =>
            {
                loggerOptions.AddGenevaLogExporter(exporterOptions =>
                {
                    exporterOptions.ConnectionString = "EtwSession=OpenTelemetry";
                    exporterOptions.TableNameMappings = new Dictionary<string, string>
                    {
                        ["DiagnosticEvent"] = "DiagnosticEventFromOpenTelemetry",
                    };
                });
            }));

            this.logger = loggerFactory.CreateLogger<GenevaLogger>();
        }

        /// <summary>
        /// Log a message.
        /// </summary>
        /// <param name="logMessage">log message.</param>
        public override void Log(ILogMessage logMessage)
        {
            _ = logMessage ?? throw new ArgumentNullException(nameof(logMessage));
            string message = logMessage.GetLogMessage(this.LogDecorations, this.LogLevels);

            switch (logMessage.LogLevel)
            {
                case Logger.LogLevels.Critical:
                    this.LogDiagnosticEvent(message, LogLevel.Critical);
                    break;

                case Logger.LogLevels.Error:
                    this.LogDiagnosticEvent(message, LogLevel.Error);
                    break;

                case Logger.LogLevels.Debug:
                    this.LogDiagnosticEvent(message, LogLevel.Debug);
                    break;

                case Logger.LogLevels.Warning:
                    this.LogDiagnosticEvent(message, LogLevel.Warning);
                    break;

                default:
                    this.LogDiagnosticEvent(message, LogLevel.Information);
                    break;
            }
        }

        /// <summary>
        /// Deinitialize logger.
        /// </summary>
        public override void DeinitLog()
        {
        }

        private void LogDiagnosticEvent(string message, LogLevel logLevel)
        {
            StackTrace st = new StackTrace(true);

            // We use stack frame = 4 since we need the previous 4th function call stack that called us.
            StackFrame funcStackFrame = st.GetFrame(4);
            int sourceLineNumber = funcStackFrame.GetFileLineNumber();
            string memberName = funcStackFrame.GetMethod().Name;
            string sourceFilePath = funcStackFrame.GetFileName();

            DiagnosticEvent diagnosticEvent = new DiagnosticEvent
            {
                Message = message,
                MemberName = memberName,
                SourceFilePath = sourceFilePath,
                SourceLineNumber = sourceLineNumber,
                TracingLevel = logLevel.ToString(),
            };

            this.logger.Log(logLevel, default, diagnosticEvent, null, DiagnosticEvent.Formatter);
        }
    }
}
