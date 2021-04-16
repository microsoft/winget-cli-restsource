// -----------------------------------------------------------------------
// <copyright file="PackageMatchFieldValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.EnumValidators
{
    using System.Collections.Generic;

    /// <summary>
    /// PackageMatchFieldValidator.
    /// </summary>
    public class PackageMatchFieldValidator : ApiEnumValidator
    {
        private List<string> enumList = new List<string>
        {
            "PackageIdentifier",
            "PackageName",
            "Moniker",
            "Command",
            "Tag",
            "PackageFamilyName",
            "ProductCode",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageMatchFieldValidator"/> class.
        /// </summary>
        public PackageMatchFieldValidator()
        {
            this.Values = this.enumList;
        }
    }
}
