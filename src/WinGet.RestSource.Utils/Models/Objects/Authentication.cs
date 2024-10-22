// -----------------------------------------------------------------------
// <copyright file="Authentication.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Objects
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Utils.Models.Core;
    using Microsoft.WinGet.RestSource.Utils.Validators;
    using Microsoft.WinGet.RestSource.Utils.Validators.EnumValidators;
    using Microsoft.WinGet.RestSource.Utils.Validators.StringValidators;

    /// <summary>
    /// Authentication.
    /// </summary>
    public class Authentication : IApiObject<Authentication>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Authentication"/> class.
        /// </summary>
        public Authentication()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Authentication"/> class.
        /// </summary>
        /// <param name="authentication">Authentication.</param>
        public Authentication(Authentication authentication)
        {
            this.Update(authentication);
        }

        /// <summary>
        /// Gets or sets AuthenticationType.
        /// </summary>
        [AuthenticationTypeValidator]
        public string AuthenticationType { get; set; }

        /// <summary>
        /// Gets or sets MicrosoftEntraIdAuthenticationInfo.
        /// </summary>
        public MicrosoftEntraIdAuthenticationInfo MicrosoftEntraIdAuthenticationInfo { get; set; }

        /// <summary>
        /// Operator==.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator ==(Authentication left, Authentication right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operator!=.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator !=(Authentication left, Authentication right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc />
        public void Update(Authentication obj)
        {
            this.AuthenticationType = obj.AuthenticationType;
            this.MicrosoftEntraIdAuthenticationInfo = obj.MicrosoftEntraIdAuthenticationInfo;
        }

        /// <inheritdoc/>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // No custom validation
            return new List<ValidationResult>();
        }

        /// <inheritdoc />
        public bool Equals(Authentication other)
        {
            return (this.AuthenticationType, this.MicrosoftEntraIdAuthenticationInfo) ==
                   (other.AuthenticationType, other.MicrosoftEntraIdAuthenticationInfo);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is Authentication auth && this.Equals(auth);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (this.AuthenticationType, this.MicrosoftEntraIdAuthenticationInfo).GetHashCode();
        }
    }
}
