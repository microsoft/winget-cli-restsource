// -----------------------------------------------------------------------
// <copyright file="AuthenticationTypeValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.EnumValidators
{
    using System.Collections.Generic;
    using Microsoft.WinGet.RestSource.Utils.Constants.Enumerations;

    /// <summary>
    /// AuthenticationTypeValidator.
    /// </summary>
    public class AuthenticationTypeValidator : ApiEnumValidator
    {
        private const bool Nullable = false;
        private List<string> enumList = new List<string>
        {
            AuthenticationType.None,
            AuthenticationType.MicrosoftEntraId,
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationTypeValidator"/> class.
        /// </summary>
        public AuthenticationTypeValidator()
        {
            this.AllowNull = Nullable;
            this.Values = this.enumList;
        }
    }
}
