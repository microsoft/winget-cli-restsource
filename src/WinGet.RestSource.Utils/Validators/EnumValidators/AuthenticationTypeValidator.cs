// -----------------------------------------------------------------------
// <copyright file="AuthenticationTypeValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.EnumValidators
{
    using System;
    using System.Collections.Generic;
    using Microsoft.WinGet.RestSource.Utils.Constants.Enumerations;
    using Microsoft.WinGet.RestSource.Utils.Models.Schemas;

    /// <summary>
    /// AuthenticationTypeValidator.
    /// </summary>
    public class AuthenticationTypeValidator : ApiEnumValidator
    {
        private const bool Nullable = false;

        private List<string> sourceValueList = new List<string>
        {
            AuthenticationType.None,
            AuthenticationType.MicrosoftEntraId,
        };

        private List<string> installerValueList = new List<string>
        {
            AuthenticationType.None,
            AuthenticationType.MicrosoftEntraId,
            AuthenticationType.MicrosoftEntraIdForAzureBlobStorage,
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationTypeValidator"/> class.
        /// </summary>
        /// <param name="authenticationClassType">The authentication class type.</param>
        public AuthenticationTypeValidator(Type authenticationClassType)
        {
            if (authenticationClassType == typeof(Information))
            {
                this.Values = this.sourceValueList;
            }
            else if (authenticationClassType == typeof(Installer))
            {
                this.Values = this.installerValueList;
            }
            else
            {
                throw new ArgumentException("The authentication class type is not supported.");
            }

            this.AllowNull = Nullable;
        }
    }
}
