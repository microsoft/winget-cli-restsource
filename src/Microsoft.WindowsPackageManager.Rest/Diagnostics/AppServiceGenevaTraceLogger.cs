// -----------------------------------------------------------------------
// <copyright file="AppServiceGenevaTraceLogger.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WindowsPackageManager.Rest.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.Cloud.InstrumentationFramework;
    using Microsoft.Msix.Utils.Logger;

    /// <summary>
    /// Log provider wrapper that logs to Geneva.
    /// </summary>
    public class AppServiceGenevaTraceLogger : LogProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppServiceGenevaTraceLogger"/> class.
        /// </summary>
        public AppServiceGenevaTraceLogger()
        {
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
                    this.DoLog(logMessage.GetLogMessage(this.LogDecorations, this.LogLevels), IfxTracingLevel.Critical);
                    break;

                case Logger.LogLevels.Error:
                    this.DoLog(logMessage.GetLogMessage(this.LogDecorations, this.LogLevels), IfxTracingLevel.Error);
                    break;

                case Logger.LogLevels.InfoPrompt:
                    this.DoLog(logMessage.GetLogMessage(this.LogDecorations, this.LogLevels), IfxTracingLevel.Informational);
                    break;

                case Logger.LogLevels.Warning:
                    this.DoLog(logMessage.GetLogMessage(this.LogDecorations, this.LogLevels), IfxTracingLevel.Warning);
                    break;

                default:
                    this.DoLog(logMessage.GetLogMessage(this.LogDecorations, this.LogLevels), IfxTracingLevel.Informational);
                    break;
            }
        }

        /// <summary>
        /// Deinitialize logger.
        /// </summary>
        public override void DeinitLog()
        {
        }

        /// <summary>
        /// Logs a DiagnosticEvent with a level, message and dictionary of key-value properties.
        /// These events will be pushed to our geneva warm path. IfxTracingLevel.Verbose logs will not be uploaded to geneva.
        /// </summary>
        /// <param name="level">Logging level.</param>
        /// <param name="message">Event name.</param>
        /// <param name="memberName">Calling member name.</param>
        /// <param name="sourceFilePath">Calling source file.</param>
        /// <param name="sourceLineNumber">Calling source file line number.</param>
        public void Log(
            IfxTracingLevel level,
            string message,
            string memberName = "",
            string sourceFilePath = "",
            int sourceLineNumber = 0)
        {
            if (TelemetryOperation.IfxInitialized && level != IfxTracingLevel.Verbose)
            {
                try
                {
                    var diagnostic = new DiagnosticEvent
                    {
                        Message = message,
                        MemberName = memberName,
                        SourceFilePath = sourceFilePath,
                        SourceLineNumber = sourceLineNumber,
                        TracingLevel = level.ToString(),
                    };
                    TelemetryOperation.SetCommonPartC(diagnostic);
                    CorrelationContext.Set(CorrelationContext.Get());
                    IfxEvent.Log(diagnostic, level);
                }
                catch (Exception ex)
                {
                    // log exception to ILogger interface if this Ifx call fails.
                    throw new ArgumentNullException("Failed to write Ifx DiagnosticEvent", ex.Message);
                }
            }
        }

        /// <summary>
        /// IfxTrace logging requires writing logs to disk on vm's. Disabling this for now
        /// and pushing all logs to the warm path as a diagnostic event.
        /// Writes a log message with a list of key-value properties.
        /// Note: Only the first eight key-value pairs are logged.
        /// </summary>
        /// <param name="level">Logging level.</param>
        /// <param name="tag">Tag descriptor.</param>
        /// <param name="keyValues">List of properties.</param>
        public void Trace(
            IfxTracingLevel level, string tag, IEnumerable<KeyValuePair<string, string>> keyValues)
        {
            if (keyValues != null)
            {
                var kv = keyValues.ToArray();
                switch (kv.Length)
                {
                    case 0:
                        break;

                    case 1:
                        IfxTracer.LogPropertyBag(
                            level, tag, kv[0].Key, kv[0].Value);
                        break;

                    case 2:
                        IfxTracer.LogPropertyBag(
                            level, tag, kv[0].Key, kv[0].Value, kv[1].Key, kv[1].Value);
                        break;

                    case 3:
                        IfxTracer.LogPropertyBag(
                            level,
                            tag,
                            kv[0].Key,
                            kv[0].Value,
                            kv[1].Key,
                            kv[1].Value,
                            kv[2].Key,
                            kv[2].Value);
                        break;

                    case 4:
                        IfxTracer.LogPropertyBag(
                            level,
                            tag,
                            kv[0].Key,
                            kv[0].Value,
                            kv[1].Key,
                            kv[1].Value,
                            kv[2].Key,
                            kv[2].Value,
                            kv[3].Key,
                            kv[3].Value);
                        break;

                    case 5:
                        IfxTracer.LogPropertyBag(
                            level,
                            tag,
                            kv[0].Key,
                            kv[0].Value,
                            kv[1].Key,
                            kv[1].Value,
                            kv[2].Key,
                            kv[2].Value,
                            kv[3].Key,
                            kv[3].Value,
                            kv[4].Key,
                            kv[4].Value);
                        break;

                    case 6:
                        IfxTracer.LogPropertyBag(
                            level,
                            tag,
                            kv[0].Key,
                            kv[0].Value,
                            kv[1].Key,
                            kv[1].Value,
                            kv[2].Key,
                            kv[2].Value,
                            kv[3].Key,
                            kv[3].Value,
                            kv[4].Key,
                            kv[4].Value,
                            kv[5].Key,
                            kv[5].Value);
                        break;

                    case 7:
                        IfxTracer.LogPropertyBag(
                            level,
                            tag,
                            kv[0].Key,
                            kv[0].Value,
                            kv[1].Key,
                            kv[1].Value,
                            kv[2].Key,
                            kv[2].Value,
                            kv[3].Key,
                            kv[3].Value,
                            kv[4].Key,
                            kv[4].Value,
                            kv[5].Key,
                            kv[5].Value,
                            kv[6].Key,
                            kv[6].Value);
                        break;

                    default:
                        IfxTracer.LogPropertyBag(
                            level,
                            tag,
                            kv[0].Key,
                            kv[0].Value,
                            kv[1].Key,
                            kv[1].Value,
                            kv[2].Key,
                            kv[2].Value,
                            kv[3].Key,
                            kv[3].Value,
                            kv[4].Key,
                            kv[4].Value,
                            kv[5].Key,
                            kv[5].Value,
                            kv[6].Key,
                            kv[6].Value,
                            kv[7].Key,
                            kv[7].Value);
                        break;
                }
            }
        }

        private void DoLog(
            string message,
            IfxTracingLevel level)
        {
            StackTrace st = new StackTrace(true);

            // We use stack frame = 4 since we need the previous 4th function call stack that called us.
            StackFrame funcStackFrame = st.GetFrame(4);
            int sourceLineNumber = funcStackFrame.GetFileLineNumber();
            string memberName = funcStackFrame.GetMethod().Name;
            string sourceFilePath = funcStackFrame.GetFileName();
            this.Log(level, message, memberName, sourceFilePath, sourceLineNumber);
        }
    }
}