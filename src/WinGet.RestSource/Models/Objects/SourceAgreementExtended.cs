// -----------------------------------------------------------------------
// <copyright file="SourceAgreementExtended.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Objects
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Constants;
    using Microsoft.WinGet.RestSource.Models.Arrays;
    using Microsoft.WinGet.RestSource.Models.Core;
    using Microsoft.WinGet.RestSource.Validators.StringValidators;

    /// <summary>
    /// SourceAgreementExtended.
    /// </summary>
    public class SourceAgreementExtended : IApiObject<SourceAgreementExtended>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SourceAgreementExtended"/> class.
        /// </summary>
        public SourceAgreementExtended()
        {
        }

        /// <summary>
        /// Gets or sets AgreementsIdentifier.
        /// </summary>
        [AgreementsIdentifierValidator]
        public string AgreementsIdentifier { get; set; }

        /// <summary>
        /// Gets or sets Agreements.
        /// </summary>
        public Agreements Agreements { get; set; }

        /// <summary>
        /// Operator==.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator ==(SourceAgreementExtended left, SourceAgreementExtended right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operator!=.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator !=(SourceAgreementExtended left, SourceAgreementExtended right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// This updates the current agreement to match the other agreement.
        /// </summary>
        /// <param name="agreement">Package Dependency.</param>
        public void Update(SourceAgreementExtended agreement)
        {
            this.AgreementsIdentifier = agreement.AgreementsIdentifier;
            this.Agreements = agreement.Agreements;
        }

        /// <inheritdoc />
        public bool Equals(SourceAgreementExtended other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this.AgreementsIdentifier, other.AgreementsIdentifier) && Equals(this.Agreements, other.Agreements);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is SourceAgreementExtended sourceAgreementExtended && this.Equals(sourceAgreementExtended);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return System.HashCode.Combine(this.Agreements, this.AgreementsIdentifier);
        }

        /// <inheritdoc/>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (this.Agreements != null)
            {
                results = (List<ValidationResult>)this.Agreements.Validate(validationContext);
            }

            return results;
        }
    }
}
