// -----------------------------------------------------------------------
// <copyright file="ExpectedReturnCodeResponseValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.EnumValidators
{
    using System.Collections.Generic;

    /// <summary>
    /// Expected Return Code Response.
    /// </summary>
    public class ExpectedReturnCodeResponseValidator : ApiEnumValidator
    {
        private const bool Nullable = false;
        private List<string> enumList = new List<string>
        {
            "packageInUse",
            "packageInUseByApplication",
            "installInProgress",
            "fileInUse",
            "missingDependency",
            "diskFull",
            "insufficientMemory",
            "noNetwork",
            "contactSupport",
            "rebootRequiredToFinish",
            "rebootRequiredForInstall",
            "rebootInitiated",
            "cancelledByUser",
            "alreadyInstalled",
            "downgrade",
            "blockedByPolicy",
            "custom",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpectedReturnCodeResponseValidator"/> class.
        /// </summary>
        public ExpectedReturnCodeResponseValidator()
        {
            this.Values = this.enumList;
            this.AllowNull = Nullable;
        }
    }
}
