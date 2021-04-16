// -----------------------------------------------------------------------
// <copyright file="MatchTypeValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.EnumValidators
{
    using System.Collections.Generic;

    /// <summary>
    /// MatchTypeValidatorSchema.
    /// </summary>
    public class MatchTypeValidator : ApiEnumValidator
    {
        private const bool Nullable = true;
        private List<string> enumList = new List<string>
        {
            "Exact",
            "CaseInsensitive",
            "StartsWith",
            "Substring",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchTypeValidator"/> class.
        /// </summary>
        public MatchTypeValidator()
        {
            this.AllowNull = Nullable;
            this.Values = this.enumList;
        }
    }
}
