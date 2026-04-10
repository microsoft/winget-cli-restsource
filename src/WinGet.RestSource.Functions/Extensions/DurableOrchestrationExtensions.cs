// -----------------------------------------------------------------------
// <copyright file="DurableOrchestrationExtensions.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Functions.Extensions
{
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Microsoft.Msix.Utils.Logger;

    /// <summary>
    /// Helper class for IDurableOrchestrationContext. Handles not logging on replays.
    /// </summary>
    public static class DurableOrchestrationExtensions
    {
        /// <summary>
        /// Log info one time in a durable context.
        /// </summary>
        /// <param name="durableContext">DurableContext.</param>
        /// <param name="message">message.</param>
        public static void LogInfo(this IDurableOrchestrationContext durableContext, string message)
        {
            if (!durableContext.IsReplaying)
            {
                Logger.Info(message);
            }
        }

        /// <summary>
        /// Log error one time in a durable context.
        /// </summary>
        /// <param name="durableContext">DurableContext.</param>
        /// <param name="message">message.</param>
        public static void LogError(this IDurableOrchestrationContext durableContext, string message)
        {
            if (!durableContext.IsReplaying)
            {
                Logger.Error(message);
            }
        }

        /// <summary>
        /// Log warning one time in a durable context.
        /// </summary>
        /// <param name="durableContext">DurableContext.</param>
        /// <param name="message">message.</param>
        public static void LogWarning(this IDurableOrchestrationContext durableContext, string message)
        {
            if (!durableContext.IsReplaying)
            {
                Logger.Warning(message);
            }
        }
    }
}
