// -----------------------------------------------------------------------
// <copyright file="InternalRestError.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Errors
{
    using System;

    /// <summary>
    /// This class represents an error response. When an error occurs, we will return this to the client.
    /// </summary>
    public class InternalRestError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InternalRestError"/> class.
        /// </summary>
        public InternalRestError()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalRestError"/> class.
        /// </summary>
        /// <param name="errorCode">Error Code.</param>
        /// <param name="errorMessage">Error Message.</param>
        /// <param name="exception">Optional Exception.</param>
        public InternalRestError(int errorCode, string errorMessage, Exception exception = null)
        {
            this.ErrorCode = errorCode;
            this.ErrorMessage = errorMessage;

            if (exception != null)
            {
                this.ErrorMessage = string.Concat(this.ErrorMessage, Environment.NewLine, exception.ToString());
            }
        }

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
