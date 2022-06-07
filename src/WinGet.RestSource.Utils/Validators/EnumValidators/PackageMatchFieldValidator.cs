// -----------------------------------------------------------------------
// <copyright file="PackageMatchFieldValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.EnumValidators
{
    using System.Collections.Generic;
    using Microsoft.WinGet.RestSource.Utils.Constants.Enumerations;

    /// <summary>
    /// PackageMatchFieldValidator.
    /// </summary>
    public class PackageMatchFieldValidator : ApiEnumValidator
    {
        private readonly List<string> enumList = new List<string>
        {
            PackageMatchFields.PackageIdentifier,
            PackageMatchFields.PackageName,
            PackageMatchFields.Publisher,
            PackageMatchFields.Moniker,
            PackageMatchFields.Command,
            PackageMatchFields.Tag,
            PackageMatchFields.PackageFamilyName,
            PackageMatchFields.ProductCode,
            PackageMatchFields.NormalizedPackageNameAndPublisher,
            PackageMatchFields.Market,
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
