// -----------------------------------------------------------------------
// <copyright file="MetricsManager.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WindowsPackageManager.Rest.Diagnostics
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Cloud.InstrumentationFramework;
    using Microsoft.Msix.Utils.Logger;

    /// <summary>
    /// Class that contains Metrics manager functionality.
    /// </summary>
    public class MetricsManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MetricsManager"/> class.
        /// </summary>
        /// <param name="monitoringAccount">Monitoring account(Should be linked to Geneva logging account by this).</param>
        public MetricsManager(string monitoringAccount)
        {
            this.MonitoringAccount = monitoringAccount;
        }

        /// <summary>
        /// Gets monitoring account.
        /// </summary>
        public string MonitoringAccount { get; private set; }

        /// <summary>
        /// Forward call to EmitMetric creating a one dimension with the operation id.
        /// </summary>
        /// <param name="metricNameSpace">Metric namespace.</param>
        /// <param name="metricName">Metric name.</param>
        /// <param name="metricValue">Metric value.</param>
        /// <param name="operationId">OperationId.</param>
        /// <param name="error">Error.</param>
        /// <param name="loggingContext">Logging Context.</param>
        public void EmitMetricForOperationId(
            string metricNameSpace,
            string metricName,
            long metricValue,
            string operationId,
            string error,
            LoggingContext loggingContext)
        {
            var dimension = new Dictionary<string, string>
            {
                { "OperationId", operationId },
                { "Error", error },
            };
            this.EmitMetric(metricNameSpace, metricName, metricValue, dimension, loggingContext);
        }

        /// <summary>
        /// Method that creates MeasureMetric.
        /// </summary>
        /// <param name="metricNameSpace">Metric namespace.</param>
        /// <param name="metricName">Metric name.</param>
        /// <param name="metricValue">Metric value.</param>
        /// <param name="dimensions">Dimensions.</param>
        /// <param name="loggingContext">Logging Context.</param>
        public void EmitMetric(
            string metricNameSpace,
            string metricName,
            long metricValue,
            Dictionary<string, string> dimensions,
            LoggingContext loggingContext)
        {
            if (string.IsNullOrEmpty(this.MonitoringAccount))
            {
                return;
            }

            if (dimensions == null)
            {
                dimensions = new Dictionary<string, string>();
            }

            var dimensionNames = dimensions.Keys.ToArray();
            var dimensionValues = dimensions.Values.ToArray();

            Logger.Info($"{loggingContext}Emitting metric '{metricName}' in namespace '{metricNameSpace}'" +
                $"with dimension names '{string.Join(", ", dimensionNames)}' values '{string.Join(", ", dimensionValues)}'");

            ErrorContext mdmError = default;

            MeasureMetric measure = MeasureMetric.Create(
                this.MonitoringAccount,
                metricNameSpace,
                metricName,
                ref mdmError,
                dimensionNames);

            if (measure == null)
            {
                Logger.Error($"{loggingContext}Fail to create MeasureMetric, error code is " +
                    $"{mdmError.ErrorCode:X}. Message '{mdmError.ErrorMessage}'");
            }

            if (!measure.LogValue(metricValue, ref mdmError, dimensionValues))
            {
                Logger.Error($"{loggingContext}Fail to set MeasureMetric value, error code is " +
                    $"{mdmError.ErrorCode:X}. Message '{mdmError.ErrorMessage}'");
            }
        }
    }
}
