// -----------------------------------------------------------------------
// <copyright file="ErrorConstants.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.Functions.Constants
{
    /// <summary>
    /// This contains the constants for errors and error codes.
    /// </summary>
    internal class ErrorConstants
    {
        /// <summary>
        /// This is the code for an unhandled error.
        /// </summary>
        public const int UnhandledErrorCode = 1;

        /// <summary>
        /// This is the message for an unhandled error.
        /// </summary>
        public const string UnhandledErrorMessage = "An unhandled error occured.";
    }
}