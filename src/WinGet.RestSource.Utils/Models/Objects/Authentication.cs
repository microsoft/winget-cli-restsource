// -----------------------------------------------------------------------
// <copyright file="Authentication.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Objects
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Models.Core;
    using Microsoft.WinGet.RestSource.Utils.Validators;
    using Microsoft.WinGet.RestSource.Utils.Validators.EnumValidators;
    using Microsoft.WinGet.RestSource.Utils.Validators.StringValidators;

    /// <summary>
    /// Authentication.
    /// </summary>
    /// <typeparam name="T">The authentication class type.</typeparam>
    public class Authentication<T> : IApiObject<Authentication<T>>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Authentication{T}"/> class.
        /// </summary>
        public Authentication()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Authentication{T}"/> class.
        /// </summary>
        /// <param name="authentication">Authentication.</param>
        public Authentication(Authentication<T> authentication)
        {
            this.Update(authentication);
        }

        /// <summary>
        /// Gets or sets AuthenticationType.
        /// </summary>
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
        public static bool operator ==(Authentication<T> left, Authentication<T> right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operator!=.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator !=(Authentication<T> left, Authentication<T> right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc />
        public void Update(Authentication<T> obj)
        {
            this.AuthenticationType = obj.AuthenticationType;
            this.MicrosoftEntraIdAuthenticationInfo = obj.MicrosoftEntraIdAuthenticationInfo;
        }

        /// <inheritdoc/>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            var authenticationTypeValidator = new AuthenticationTypeValidator(typeof(T));
            authenticationTypeValidator.Validate(this.AuthenticationType, validationContext);

            if (Constants.Enumerations.AuthenticationType.MicrosoftEntraId.Equals(this.AuthenticationType, StringComparison.OrdinalIgnoreCase))
            {
                if (this.MicrosoftEntraIdAuthenticationInfo == null)
                {
                    results.Add(new ValidationResult($"{nameof(this.MicrosoftEntraIdAuthenticationInfo)} cannot be null if {nameof(this.AuthenticationType)} is microsoftEntraId"));
                }

                ApiDataValidator.Validate(this.MicrosoftEntraIdAuthenticationInfo, results);
            }

            return results;
        }

        /// <inheritdoc />
        public bool Equals(Authentication<T> other)
        {
            return (this.AuthenticationType, this.MicrosoftEntraIdAuthenticationInfo) ==
                   (other.AuthenticationType, other.MicrosoftEntraIdAuthenticationInfo);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is Authentication<T> auth && this.Equals(auth);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (this.AuthenticationType, this.MicrosoftEntraIdAuthenticationInfo).GetHashCode();
        }
    }
}
