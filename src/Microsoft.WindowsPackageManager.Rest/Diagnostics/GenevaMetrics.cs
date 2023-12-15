// -----------------------------------------------------------------------
// <copyright file="GenevaMetrics.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WindowsPackageManager.Rest.Diagnostics
{
    using System.Collections.Generic;
    using System.Diagnostics.Metrics;
    using System.Linq;
    using Microsoft.Msix.Utils.Logger;
    using Microsoft.WindowsPackageManager.Rest.Diagnostics;
    using OpenTelemetry;
    using OpenTelemetry.Exporter.Geneva;
    using OpenTelemetry.Metrics;

    /// <summary>
    /// Class that contains the Geneva Metrics functionality.
    /// </summary>
    public class GenevaMetrics
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenevaMetrics"/> class.
        /// </summary>
        /// <param name="monitoringAccount">Monitoring account(Should be linked to Geneva logging account by this).</param>
        public GenevaMetrics(string monitoringAccount)
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

            Meter meter = new Meter($"{this.MonitoringAccount}.{metricNameSpace}");
            Counter<long> meterCounter = meter.CreateCounter<long>($"{metricName}");

            var meterProvider = Sdk.CreateMeterProviderBuilder()
                .AddMeter(meter.Name)
                .AddGenevaMetricExporter(options =>
                {
                    options.ConnectionString = $"Account={this.MonitoringAccount};Namespace={metricNameSpace}";
                })
                .Build();

            meterCounter.Add(metricValue, dimensions.Select(pair => new KeyValuePair<string, object>(pair.Key, pair.Value)).ToArray());

            meterProvider.Dispose();
        }
    }
}
