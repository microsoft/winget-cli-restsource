// -----------------------------------------------------------------------
// <copyright file="SourceResultType.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WindowsPackageManager.Rest.Models
{
    /// <summary>
    /// Type of source results.
    /// </summary>
    public enum SourceResultType
    {
        /// <summary>
        /// The source operation was a success.
        /// </summary>
        Success,

        /// <summary>
        /// The source operation resulted in an error.
        /// </summary>
        Error,

        /// <summary>
        /// The source operation failed and should not be retried.
        /// </summary>
        Failure,

        /// <summary>
        /// The source operation was a failure and can be retried. Generally an Azure error or any network issue.
        /// </summary>
        FailureCanRetry,
    }
}
