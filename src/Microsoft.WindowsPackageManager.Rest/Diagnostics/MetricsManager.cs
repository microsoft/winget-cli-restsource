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
    using Microsoft.WindowsPackageManager.Rest.Diagnostics;

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
        public string MonitoringAccount
        {
            get; private set;
        }

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
            var dimension = new Dictionary<string, string>();
            dimension.Add("OperationId", operationId);
            dimension.Add("Error", error);
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

            ErrorContext mdmError = default;

            if (dimensions == null)
            {
                dimensions = new Dictionary<string, string>();
            }

            var dimensionNames = dimensions.Keys.ToArray();
            var dimensionValues = dimensions.Values.ToArray();

            // There are multiple limitations to IFx Metrics. If you see an error (mostly 0x80070057 / 2147942487 E_INVALIDARG)
            // in the LogValue call verify:
            //  - Metric value is < 0
            //  - Number of dimensions > 64
            //  - monitoringAccount, metricNamespace or metricName is null or empty.
            //  - monitoringAccount, metricNamespace or metricName length have exceeded the below mentioned limits.
            //  - Number of dimensions is 0 but passed in dimensionNames or dimensionValues is not null.
            //  - Number of dimensions is > 0 but passed in dimensionNames or dimensionValues is null.
            //  - dimensionName is null or empty.
            //  - dimensionValue is null.
            //  - dimensionName or dimensionValue exceed below mentioned limits.
            //  - string arguments contain non-printable control characters
            // The maximum number of user defined dimensions for each metric.
            //   MAX_NUMBER_DIMENSIONS = 64
            // The maximum number of default dimensions for each metric. Total number of supported dimension is 74 (user defined + default dimension)
            //   MAX_NUMBER_DEFAULT_DIMENSIONS = 10
            // The maximum length of the metric namespace string.
            //   MAX_NAMESPACE_SIZE < 1024
            // The maximum length of the name string, currently this applies to metric name as well as dimension names.
            //   MAX_NAME_SIZE < 256
            //   MAX_DIMENSION_NAME_SIZE = MAX_NAME_SIZE
            // The maximum length of the dimension value string.
            //   MAX_DIMENSION_VALUE_SIZE < 1024
            Logger.Info($"{loggingContext}Emitting metric '{metricName}' in namespace '{metricNameSpace}'" +
                $"with dimension names '{string.Join(", ", dimensionNames)}' values '{string.Join(", ", dimensionValues)}'");

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
