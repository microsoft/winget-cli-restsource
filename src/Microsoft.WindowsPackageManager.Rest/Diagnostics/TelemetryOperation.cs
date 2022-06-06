// -----------------------------------------------------------------------
// <copyright file="TelemetryOperation.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WindowsPackageManager.Rest.Diagnostics
{
    using System;
    using System.Net;
    using Microsoft.Cloud.InstrumentationFramework;

    /// <summary>
    /// Class that contains telemetry operation functionality.
    /// </summary>
    public class TelemetryOperation
    {
        /// <summary>
        /// Gets a value indicating whether Ifx is initialized.
        /// </summary>
        public static bool IfxInitialized { get; private set; }

        private static CommonEventProperties CommonEventProperties { get; set; }

        /// <summary>
        /// Initialize method which should be called from the Main method in Program.cs of the service.
        /// </summary>
        /// <param name="monitorRole">variable that holds information about the role.</param>
        /// <param name="monitorTenant">variable that holds information about the tenant.</param>
        /// <param name="appServiceEnvironment">variable that holds information about the app service environment.</param>
        public static void Initialize(
            string monitorRole,
            string monitorTenant,
            string appServiceEnvironment)
        {
            CommonEventProperties = new CommonEventProperties(appServiceEnvironment);

            var isDevelopmentEnvironment = appServiceEnvironment == "Development";

            // Only initialize Ifx if running in staging or production environment
            if (!isDevelopmentEnvironment)
            {
                // https://genevamondocs.azurewebsites.net/collect/references/antares/ifx.html
                // strings tenant and role are assumed to have been retrieved from web config.  They must match the parameters used
                // to start the agent or else the agent and IFx will not be able to communicate.
                // RoleInstance is dynamic so it must be discovered at runtime before calling IFxInitialize.
                var monitorRoleInstance = GetRoleInstance();

                // TODO: TASK 5543692: Update the trace directory to store traces in files and upload trace files to xStore.
                string traceDirectory = "NO_DISK_LOGS";
                uint maxSizeTraceFileInKB = 20;
                uint maxSizeCompletedTraceFilesInMB = 50;
                var auditSpecification = new AuditSpecification();
                var instrumentationSpecification = new InstrumentationSpecification
                {
                    TraceDirectory = traceDirectory,
                    MaxSizeTraceFileInKb = maxSizeTraceFileInKB,
                    MaxSizeCompletedTraceFilesInMb = maxSizeCompletedTraceFilesInMB,
                };

                IfxInitializer.IfxInitialize(
                   monitorTenant, monitorRole, monitorRoleInstance, instrumentationSpecification, auditSpecification);
                IfxInitialized = true;
            }
            else
            {
                IfxInitialized = false;
            }
        }

        /// <summary>
        /// Set common part method.
        /// </summary>
        /// <typeparam name="T">Type of AppService Ifx event.</typeparam>
        /// <param name="appServiceIfxEvent">AppService Ifx event.</param>
        public static void SetCommonPartC<T>(T appServiceIfxEvent)
            where T : AppServiceBaseEvent
        {
            if (appServiceIfxEvent != null)
            {
                // set all common part c fields here for each operation
                appServiceIfxEvent.AspEnvironment = CommonEventProperties.AppServiceEnvironment;
            }
        }

        private static string GetRoleInstance()
        {
            string ipAddress = null;
            try
            {
                IPAddress[] addresses = Dns.GetHostAddresses(Environment.MachineName);
                foreach (var addr in addresses)
                {
                    if (addr.ToString() == "127.0.0.1")
                    {
                        continue;
                    }
                    else if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        ipAddress = addr.ToString();
                        break;
                    }
                }
            }
            catch (Exception)
            {
            }

            return ipAddress;
        }
    }
}
