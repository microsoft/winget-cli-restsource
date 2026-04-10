// -----------------------------------------------------------------------
// <copyright file="Documentation.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Objects
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Utils.Models.Core;
    using Microsoft.WinGet.RestSource.Utils.Validators.StringValidators;

    /// <summary>
    /// Documentation.
    /// </summary>
    public class Documentation : IApiObject<Documentation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Documentation"/> class.
        /// </summary>
        public Documentation()
        {
        }

        /// <summary>
        /// Gets or sets DocumentLabel.
        /// </summary>
        [DocumentLabelValidator]
        public string DocumentLabel { get; set; }

        /// <summary>
        /// Gets or sets DocumentUrl.
        /// </summary>
        [UrlValidator]
        public string DocumentUrl { get; set; }

        /// <summary>
        /// Operator==.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator ==(Documentation left, Documentation right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operator!=.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator !=(Documentation left, Documentation right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// This updates the current Documentation to match the other Documentation.
        /// </summary>
        /// <param name="documentation">Documentation.</param>
        public void Update(Documentation documentation)
        {
            this.DocumentLabel = documentation.DocumentLabel;
            this.DocumentUrl = documentation.DocumentUrl;
        }

        /// <inheritdoc />
        public bool Equals(Documentation other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this.DocumentLabel, other.DocumentLabel) && Equals(this.DocumentUrl, other.DocumentUrl);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is Documentation documentation && this.Equals(documentation);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (this.DocumentLabel, this.DocumentUrl).GetHashCode();
        }

        /// <inheritdoc/>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
