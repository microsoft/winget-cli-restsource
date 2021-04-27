// -----------------------------------------------------------------------
// <copyright file="PackageMatchFieldValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.EnumValidators
{
    using System.Collections.Generic;
    using Microsoft.WinGet.RestSource.Constants.Enumerations;

    /// <summary>
    /// PackageMatchFieldValidator.
    /// </summary>
    public class PackageMatchFieldValidator : ApiEnumValidator
    {
        private List<string> enumList = new List<string>
        {
            PackageMatchFields.PackageIdentifier,
            PackageMatchFields.PackageName,
            PackageMatchFields.Moniker,
            PackageMatchFields.Command,
            PackageMatchFields.Tag,
            PackageMatchFields.PackageFamilyName,
            PackageMatchFields.ProductCode,
            PackageMatchFields.NormalizedPackageNameAndPublisher,
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
