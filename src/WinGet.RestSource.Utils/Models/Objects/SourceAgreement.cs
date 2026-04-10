// -----------------------------------------------------------------------
// <copyright file="SourceAgreement.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Objects
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Models.Core;
    using Microsoft.WinGet.RestSource.Utils.Validators.StringValidators;

    /// <summary>
    /// SourceAgreement.
    /// </summary>
    public class SourceAgreement : IApiObject<SourceAgreement>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SourceAgreement"/> class.
        /// </summary>
        public SourceAgreement()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceAgreement"/> class.
        /// </summary>
        /// <param name="agreement">Agreement.</param>
        public SourceAgreement(SourceAgreement agreement)
        {
            this.Update(agreement);
        }

        /// <summary>
        /// Gets or sets AgreementLabel.
        /// </summary>
        [AgreementLabelValidator]
        public string AgreementLabel { get; set; }

        /// <summary>
        /// Gets or sets Agreement.
        /// </summary>
        [AgreementValidator]
        public string Agreement { get; set; }

        /// <summary>
        /// Gets or sets AgreementUrl.
        /// </summary>
        [UrlValidator]
        public string AgreementUrl { get; set; }

        /// <summary>
        /// Operator==.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator ==(SourceAgreement left, SourceAgreement right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operator!=.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator !=(SourceAgreement left, SourceAgreement right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// This updates the current package core to match another public core.
        /// </summary>
        /// <param name="agreement">Package Dependency.</param>
        public void Update(SourceAgreement agreement)
        {
            this.AgreementLabel = agreement.AgreementLabel;
            this.Agreement = agreement.Agreement;
            this.AgreementUrl = agreement.AgreementUrl;
        }

        /// <inheritdoc/>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }

        /// <inheritdoc />
        public bool Equals(SourceAgreement other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this.AgreementLabel, other.AgreementLabel) && Equals(this.Agreement, other.Agreement) && Equals(this.AgreementUrl, other.AgreementUrl);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is SourceAgreement sourceAgreement && this.Equals(sourceAgreement);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (this.AgreementLabel, this.Agreement, this.AgreementUrl).GetHashCode();
        }
    }
}
