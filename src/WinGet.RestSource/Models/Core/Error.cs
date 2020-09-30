// -----------------------------------------------------------------------
// <copyright file="Error.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Core
{
    /// <summary>
    /// This class represents an error response. When an error occurs, we will return this to the client.
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Gets or sets the integer error code for an error.
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the error message for an error.
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}