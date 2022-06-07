// -----------------------------------------------------------------------
// <copyright file="CommonEventProperties.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WindowsPackageManager.Rest.Diagnostics
{
    /// <summary>
    /// Class to store common values for the telemetry events derived from AppServiceBaseEvent.
    /// </summary>
    public class CommonEventProperties
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommonEventProperties"/> class.
        /// </summary>
        /// <param name="appServiceEnvironment">Environment for the events during this runtime.</param>
        public CommonEventProperties(string appServiceEnvironment)
        {
            this.AppServiceEnvironment = appServiceEnvironment;
        }

        /// <summary>
        /// Gets an app service environment.
        /// </summary>
        public string AppServiceEnvironment { get; private set; }
    }
}
