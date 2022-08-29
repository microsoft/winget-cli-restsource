// -----------------------------------------------------------------------
// <copyright file="UnsupportedArgumentValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.EnumValidators
{
    using System.Collections.Generic;

    /// <summary>
    /// UnsupportedArgumentValidator.
    /// </summary>
    public class UnsupportedArgumentValidator : ApiEnumValidator
    {
        private const bool Nullable = true;
        private List<string> enumList = new List<string>
        {
            "log",
            "location",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedArgumentValidator"/> class.
        /// </summary>
        public UnsupportedArgumentValidator()
        {
            this.AllowNull = Nullable;
            this.Values = this.enumList;
        }
    }
}
