// -----------------------------------------------------------------------
// <copyright file="Metrics.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Functions.Geneva
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    using Microsoft.WindowsPackageManager.Rest.Diagnostics;
    using Microsoft.WindowsPackageManager.Rest.Utils;
    using Microsoft.WinGet.RestSource.Functions.Constants;

    /// <summary>
    /// Helper that emits metrics or logs that trigger IcMs.
    /// </summary>
    public class Metrics
    {
        /// <summary>
        /// Wrapper around MetricsManager EmitMetricForOperation.
        /// </summary>
        /// <param name="errorMetric">Metric.</param>
        /// <param name="functionName">Operation.</param>
        /// <param name="path">Route.</param>
        /// <param name="headers">Headers.</param>
        /// <param name="error">Error.</param>
        /// <param name="log">ILogger log.</param>
        public static void EmitMetricForOperation(
            ErrorMetrics errorMetric,
            string functionName,
            string path,
            Dictionary<string, string> headers,
            Exception error,
            ILogger log)
        {
            EmitMetricForOperation(errorMetric, functionName, path, headers, null, error, log);
        }

        /// <summary>
        /// Wrapper around MetricsManager EmitMetricForOperation.
        /// </summary>
        /// <param name="errorMetric">Metric.</param>
        /// <param name="functionName">Operation.</param>
        /// <param name="path">Route.</param>
        /// <param name="headers">Headers.</param>
        /// <param name="obj">Object.</param>
        /// <param name="error">Error.</param>
        /// <param name="log">ILogger log.</param>
        public static void EmitMetricForOperation(
            ErrorMetrics errorMetric,
            string functionName,
            string path,
            Dictionary<string, string> headers,
            object obj,
            Exception error,
            ILogger log)
        {
            log.LogError($"Emitting metric {errorMetric} for function {functionName} on route {path}'");

            var dimension = new Dictionary<string, string>
            {
                { "FunctionName", functionName },
                { "Route", path },
                { "Headers", JsonHelper.SerializeObject(headers) },
                { "Object", JsonHelper.SerializeObject(obj) },
                { "Error", JsonHelper.SerializeObject(error) },
            };
            EmitMetric(errorMetric, dimension, log);
        }

        /// <summary>
        /// Wrapper around MetricsManager EmitMetric.
        /// </summary>
        /// <param name="errorMetric">Metric.</param>
        /// <param name="icmDimensions">Dimensions.</param>
        /// <param name="log">ILogger log.</param>
        /// <param name="metricValue">Metric Value.</param>
        /// <param name="metricNamespaces">Metrics Namespace.</param>
        public static void EmitMetric(
            ErrorMetrics errorMetric,
            Dictionary<string, string> icmDimensions,
            ILogger log,
            int metricValue = 1,
            MetricNamespaces metricNamespaces = MetricNamespaces.MsPkgMgrProdLogs)
        {
            log.LogError($"Emitting metric '{errorMetric}'");

            MetricsManager metricsManager = new MetricsManager(AzureFunctionEnvironment.MonitoringAccount);

            AddTenant(ref icmDimensions, log);
            AddRole(ref icmDimensions, log);

            log.LogError(JsonHelper.SerializeObject(icmDimensions));
            metricsManager.EmitMetric(
                metricNamespaces.ToString(),
                errorMetric.ToString(),
                metricValue,
                icmDimensions,
                null);
        }

        private static void AddTenant(ref Dictionary<string, string> dimensions, ILogger log)
        {
            if (!string.IsNullOrWhiteSpace(AzureFunctionEnvironment.MonitorTenant))
            {
                dimensions.Add(nameof(AzureFunctionEnvironment.MonitorTenant), AzureFunctionEnvironment.MonitorTenant);
            }
            else
            {
                log.LogError($"Fatal error {nameof(AzureFunctionEnvironment.MonitorTenant)} is empty");
            }
        }

        private static void AddRole(ref Dictionary<string, string> dimensions, ILogger log)
        {
            if (!string.IsNullOrWhiteSpace(AzureFunctionEnvironment.MonitorRole))
            {
                dimensions.Add(nameof(AzureFunctionEnvironment.MonitorRole), AzureFunctionEnvironment.MonitorRole);
            }
            else
            {
                log.LogError($"Fatal error {nameof(AzureFunctionEnvironment.MonitorRole)} is empty");
            }
        }
    }
}