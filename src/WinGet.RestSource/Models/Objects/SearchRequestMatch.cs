// -----------------------------------------------------------------------
// <copyright file="SearchRequestMatch.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Objects
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Constants;
    using Microsoft.WinGet.RestSource.Models.Core;
    using Microsoft.WinGet.RestSource.Validators.EnumValidators;
    using Microsoft.WinGet.RestSource.Validators.StringValidators;

    /// <summary>
    /// SearchRequestMatch.
    /// </summary>
    public class SearchRequestMatch : IApiObject<SearchRequestMatch>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchRequestMatch"/> class.
        /// </summary>
        public SearchRequestMatch()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchRequestMatch"/> class.
        /// </summary>
        /// <param name="searchRequestMatch">SearchRequestMatch.</param>
        public SearchRequestMatch(SearchRequestMatch searchRequestMatch)
        {
            this.Update(searchRequestMatch);
        }

        /// <summary>
        /// Gets or sets KeyWord.
        /// </summary>
        [KeyWordValidator]
        public string KeyWord { get; set; }

        /// <summary>
        /// Gets or sets MatchType.
        /// </summary>
        [MatchTypeValidator]
        public string MatchType { get; set; }

        /// <summary>
        /// Operator==.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator ==(SearchRequestMatch left, SearchRequestMatch right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operator!=.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator !=(SearchRequestMatch left, SearchRequestMatch right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// This updates the current package core to match another public core.
        /// </summary>
        /// <param name="searchRequestMatch">Package Dependency.</param>
        public void Update(SearchRequestMatch searchRequestMatch)
        {
            this.MatchType = searchRequestMatch.MatchType;
            this.KeyWord = searchRequestMatch.KeyWord;
        }

        /// <inheritdoc/>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }

        /// <inheritdoc />
        public bool Equals(SearchRequestMatch other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this.MatchType, other.MatchType) && Equals(this.KeyWord, other.KeyWord);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((SearchRequestMatch)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.MatchType != null ? this.MatchType.GetHashCode() : 0) * ApiConstants.HashCodeConstant) ^ (this.KeyWord != null ? this.KeyWord.GetHashCode() : 0);
            }
        }
    }
}
