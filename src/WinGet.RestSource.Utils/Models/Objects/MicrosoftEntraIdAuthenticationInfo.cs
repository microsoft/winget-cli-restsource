// -----------------------------------------------------------------------
// <copyright file="MicrosoftEntraIdAuthenticationInfo.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Objects
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Models.Core;
    using Microsoft.WinGet.RestSource.Utils.Validators;
    using Microsoft.WinGet.RestSource.Utils.Validators.EnumValidators;
    using Microsoft.WinGet.RestSource.Utils.Validators.StringValidators;

    /// <summary>
    /// MicrosoftEntraIdAuthenticationInfo.
    /// </summary>
    public class MicrosoftEntraIdAuthenticationInfo : IApiObject<MicrosoftEntraIdAuthenticationInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftEntraIdAuthenticationInfo"/> class.
        /// </summary>
        public MicrosoftEntraIdAuthenticationInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftEntraIdAuthenticationInfo"/> class.
        /// </summary>
        /// <param name="authInfo">MicrosoftEntraIdAuthenticationInfo.</param>
        public MicrosoftEntraIdAuthenticationInfo(MicrosoftEntraIdAuthenticationInfo authInfo)
        {
            this.Update(authInfo);
        }

        /// <summary>
        /// Gets or sets Resource.
        /// </summary>
        [MicrosoftEntraIdResourceValidator]
        public string Resource { get; set; }

        /// <summary>
        /// Gets or sets Scope.
        /// </summary>
        [MicrosoftEntraIdResourceScopeValidator]
        public string Scope { get; set; }

        /// <summary>
        /// Operator==.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator ==(MicrosoftEntraIdAuthenticationInfo left, MicrosoftEntraIdAuthenticationInfo right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operator!=.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator !=(MicrosoftEntraIdAuthenticationInfo left, MicrosoftEntraIdAuthenticationInfo right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc />
        public void Update(MicrosoftEntraIdAuthenticationInfo obj)
        {
            this.Resource = obj.Resource;
            this.Scope = obj.Scope;
        }

        /// <inheritdoc/>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // No custom validation
            return new List<ValidationResult>();
        }

        /// <inheritdoc />
        public bool Equals(MicrosoftEntraIdAuthenticationInfo other)
        {
            return (this.Resource, this.Scope) == (other.Resource, other.Scope);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is MicrosoftEntraIdAuthenticationInfo authInfo && this.Equals(authInfo);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (this.Resource, this.Scope).GetHashCode();
        }
    }
}
