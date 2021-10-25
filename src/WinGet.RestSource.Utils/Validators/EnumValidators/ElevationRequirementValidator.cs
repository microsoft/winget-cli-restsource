// -----------------------------------------------------------------------
// <copyright file="ElevationRequirementValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.EnumValidators
{
    using System.Collections.Generic;

    /// <summary>
    /// ElevationRequirementValidator.
    /// </summary>
    public class ElevationRequirementValidator : ApiEnumValidator
    {
        private const bool Nullable = true;
        private List<string> enumList = new List<string>
        {
            "elevationRequired",
            "elevationProhibited",
            "elevatesSelf",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="ElevationRequirementValidator"/> class.
        /// </summary>
        public ElevationRequirementValidator()
        {
            this.AllowNull = Nullable;
            this.Values = this.enumList;
        }
    }
}
