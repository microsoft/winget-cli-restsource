// -----------------------------------------------------------------------
// <copyright file="Icon.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Objects
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Utils.Models.Core;
    using Microsoft.WinGet.RestSource.Utils.Validators.EnumValidators;
    using Microsoft.WinGet.RestSource.Utils.Validators.StringValidators;

    /// <summary>
    /// Icon.
    /// </summary>
    public class Icon : IApiObject<Icon>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Icon"/> class.
        /// </summary>
        public Icon()
        {
        }

        /// <summary>
        /// Gets or sets IconUrl.
        /// </summary>
        [UrlValidator(Nullable = false)]
        public string IconUrl { get; set; }

        /// <summary>
        /// Gets or sets IconUrl.
        /// </summary>
        [IconFileTypeValidator]
        public string IconFileType { get; set; }

        /// <summary>
        /// Gets or sets IconUrl.
        /// </summary>
        [IconResolutionValidator]
        public string IconResolution { get; set; }

        /// <summary>
        /// Gets or sets IconUrl.
        /// </summary>
        [IconThemeValidator]
        public string IconTheme { get; set; }

        /// <summary>
        /// Gets or sets IconSha256.
        /// </summary>
        [Sha256Validator]
        public string IconSha256 { get; set; }

        /// <summary>
        /// Operator==.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator ==(Icon left, Icon right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operator!=.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator !=(Icon left, Icon right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// This updates the current Icon to match the other Icon.
        /// </summary>
        /// <param name="icon">Icon.</param>
        public void Update(Icon icon)
        {
            this.IconUrl = icon.IconUrl;
            this.IconFileType = icon.IconFileType;
            this.IconResolution = icon.IconResolution;
            this.IconTheme = icon.IconTheme;
            this.IconSha256 = icon.IconSha256;
        }

        /// <inheritdoc />
        public bool Equals(Icon other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this.IconUrl, other.IconUrl) &&
                Equals(this.IconFileType, other.IconFileType) &&
                Equals(this.IconResolution, other.IconResolution) &&
                Equals(this.IconTheme, other.IconTheme) &&
                Equals(this.IconSha256, other.IconSha256);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is Icon icon && this.Equals(icon);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (this.IconUrl, this.IconFileType, this.IconResolution, this.IconTheme, this.IconSha256).GetHashCode();
        }

        /// <inheritdoc/>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
