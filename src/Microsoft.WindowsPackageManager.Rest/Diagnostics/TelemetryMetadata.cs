// -----------------------------------------------------------------------
// <copyright file="TelemetryMetadata.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.OWCUtils.Diagnostics
{
    /// <summary>
    /// Class that contains telemetry metadata read on function startup.
    /// </summary>
    public class TelemetryMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryMetadata"/> class.
        /// </summary>
        /// <param name="telemetryRole">Role with which to configure telemetry.</param>
        /// <param name="telemetryTenant">Tenant with which to configure telemetry.</param>
        /// <param name="appServiceEnvironment">App service environment with which to configure telemetry.</param>
        public TelemetryMetadata(
            string telemetryRole,
            string telemetryTenant,
            string appServiceEnvironment)
        {
            this.TelemetryRole = telemetryRole;
            this.TelemetryTenant = telemetryTenant;
            this.AppServiceEnvironment = appServiceEnvironment;
        }

        /// <summary>
        /// Gets a value indicating the role.
        /// </summary>
        public string TelemetryRole { get; private set; }

        /// <summary>
        /// Gets a value indicating the tenant.
        /// </summary>
        public string TelemetryTenant { get; private set; }

        /// <summary>
        /// Gets a value indicating the app service environment.
        /// </summary>
        public string AppServiceEnvironment { get; private set; }
    }
}
