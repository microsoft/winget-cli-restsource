// -----------------------------------------------------------------------
// <copyright file="ErrorConstants.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Constants
{
    /// <summary>
    /// This contains the constants for errors and error codes.
    /// </summary>
    public class ErrorConstants
    {
        /// <summary>
        /// This is the code for an unhandled error.
        /// </summary>
        public const int UnhandledErrorCode = 1;

        /// <summary>
        /// This is the message for an unhandled error.
        /// </summary>
        public const string UnhandledErrorMessage = "An unhandled error occured.";

        /// <summary>
        /// This is the code for when the resource ID does not match the document ID.
        /// </summary>
        public const int IdDoesNotMatchErrorCode = 2;

        /// <summary>
        /// This is the message for when the resource ID does not match the document ID.
        /// </summary>
        public const string IdDoesNotMatchErrorMessage = "The document ID does not match resource ID.";

        /// <summary>
        /// This is the code for when the object contains no versions.
        /// </summary>
        public const int VersionsIsNullErrorCode = 3;

        /// <summary>
        /// This is the message for when the object contains no versions.
        /// </summary>
        public const string VersionsIsNullErrorMessage = "The selected package contains no version information.";

        /// <summary>
        /// This is the code for when a version does not contain the selected package.
        /// </summary>
        public const int VersionDoesNotExistErrorCode = 4;

        /// <summary>
        /// This is the message for when a version does not contain the selected package.
        /// </summary>
        public const string VersionDoesNotExistErrorMessage = "The version for the selected package does not exist.";

        /// <summary>
        /// This is the code for when a Version already exists.
        /// </summary>
        public const int VersionAlreadyExistsErrorCode = 5;

        /// <summary>
        /// This is the message for when a Version already exists.
        /// </summary>
        public const string VersionAlreadyExistsErrorMessage = "The specified version already exists.";

        /// <summary>
        /// This is the code for when the resource version does not match the document version.
        /// </summary>
        public const int VersionDoesNotMatchErrorCode = 6;

        /// <summary>
        /// This is the message for when the resource version does not match the document version.
        /// </summary>
        public const string VersionDoesNotMatchErrorMessage = "The document Version does not match resource Version.";

        /// <summary>
        /// This is the code for when the object contains no versions.
        /// </summary>
        public const int InstallerIsNullErrorCode = 7;

        /// <summary>
        /// This is the message for when the object contains no versions.
        /// </summary>
        public const string InstallerIsNullErrorMessage = "The selected version contains no installer information.";

        /// <summary>
        /// This is the code for when an installer does not contain the selected package.
        /// </summary>
        public const int InstallerDoesNotExistErrorCode = 8;

        /// <summary>
        /// This is the message for when an installer does not contain the selected package.
        /// </summary>
        public const string InstallerDoesNotExistErrorMessage =
            "The installer for the selected version does not exist.";

        /// <summary>
        /// This is the code for when a Version already exists.
        /// </summary>
        public const int InstallerAlreadyExistsErrorCode = 9;

        /// <summary>
        /// This is the message for when a Version already exists.
        /// </summary>
        public const string InstallerAlreadyExistsErrorMessage = "The specified installer already exists.";

        /// <summary>
        /// This is the code for when the installer version does not match the document installer.
        /// </summary>
        public const int InstallerDoesNotMatchErrorCode = 10;

        /// <summary>
        /// This is the message for when the resource installer does not match the document installer.
        /// </summary>
        public const string InstallerDoesNotMatchErrorMessage =
            "The document Installer does not match resource Installer.";

        /// <summary>
        /// This is the code for an Http Request Exception.
        /// </summary>
        public const int HttpRequestExceptionErrorCode = 11;

        /// <summary>
        /// This is the message for an Http Request Exception.
        /// </summary>
        public const string HttpRequestExceptionErrorMessage = "An HTTP Request Exception Occured.";

        /// <summary>
        /// This is the code for a resource Conflict.
        /// </summary>
        public const int ResourceConflictErrorCode = 409;

        /// <summary>
        /// This is the message for a resource Conflict.
        /// </summary>
        public const string ResourceConflictErrorMessage = "A resource conflict exists for this operation.";

        /// <summary>
        /// This is the code for a resource not found.
        /// </summary>
        public const int ResourceNotFoundErrorCode = 404;

        /// <summary>
        /// This is the message for a resource not found.
        /// </summary>
        public const string ResourceNotFoundErrorMessage = "The selected resource was not found.";

        /// <summary>
        /// This is the code for a failed precondition.
        /// </summary>
        public const int PreconditionFailedErrorCode = 412;

        /// <summary>
        /// This is the message for a failed precondition.
        /// </summary>
        public const string PreconditionFailedErrorMessage =
            "Operation cannot be performed because one of the specified precondition is not met. " +
            "This may have been caused by a resource changing before your request could be processed.";
    }
}
