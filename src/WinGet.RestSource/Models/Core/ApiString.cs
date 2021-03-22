// -----------------------------------------------------------------------
// <copyright file="ApiString.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Core
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;
    using Microsoft.WinGet.RestSource.Serializers;
    using Newtonsoft.Json;

    /// <summary>
    /// API String.
    /// </summary>
    [JsonConverter(typeof(ApiStringSerializer))]
    public class ApiString : IApiData<ApiString>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiString"/> class.
        /// </summary>
        public ApiString()
        {
            this.SetDefaults();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiString"/> class.
        /// </summary>
        /// <param name="apiString">API String.</param>
        public ApiString(string apiString)
        {
            this.SetDefaults();
            this.APIString = apiString;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiString"/> class.
        /// </summary>
        /// <param name="apiString">API String.</param>
        public ApiString(ApiString apiString)
        {
            this.APIStringName = apiString.APIStringName;
            this.APIString = apiString.APIString;
            this.AllowNull = apiString.AllowNull;
            this.MatchPattern = apiString.MatchPattern;
            this.MinLength = apiString.MinLength;
            this.MaxLength = apiString.MaxLength;
        }

        /// <summary>
        /// Gets or sets APIStringName.
        /// </summary>
        public string APIStringName { get; set; }

        /// <summary>
        /// Gets or sets APIString.
        /// </summary>
        public string APIString { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to Allow Null.
        /// </summary>
        protected bool AllowNull { get; set; }

        /// <summary>
        /// Gets or sets MatchPattern.
        /// </summary>
        protected string MatchPattern { get; set; }

        /// <summary>
        /// Gets or sets MinLength.
        /// </summary>
        protected uint MinLength { get; set; }

        /// <summary>
        /// Gets or sets MaxLength.
        /// </summary>
        protected uint MaxLength { get; set; }

        /// <summary>
        /// Implicit String.
        /// </summary>
        /// <param name="apiString">Api String.</param>
        /// <returns>String.</returns>
        public static implicit operator string(ApiString apiString)
        {
            return apiString.APIString;
        }

        /// <summary>
        /// Operator==.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator ==(ApiString left, ApiString right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operator!=.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator !=(ApiString left, ApiString right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// ToString.
        /// </summary>
        /// <returns>String.</returns>
        public override string ToString()
        {
            return this.APIString;
        }

        /// <summary>
        /// SetString.
        /// </summary>
        /// <param name="str">string.</param>
        public void SetString(string str)
        {
            this.APIString = str;
        }

        /// <summary>
        /// Validate.
        /// </summary>
        /// <param name="validationContext">Validation Context.</param>
        /// <returns>Validation Results.</returns>
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (this.AllowNull && string.IsNullOrEmpty(this.APIString))
            {
                return results;
            }

            if (!this.AllowNull && string.IsNullOrEmpty(this.APIString))
            {
                results.Add(new ValidationResult($"{this.APIStringName} must not be null."));
                return results;
            }

            if (!string.IsNullOrEmpty(this.MatchPattern) && !string.IsNullOrEmpty(this.APIString))
            {
                if (!Regex.IsMatch(this.APIString, this.MatchPattern))
                {
                    results.Add(new ValidationResult($"{this.APIStringName} '{this.APIString}' must match validation pattern: '{this.MatchPattern}'."));
                }
            }

            if (this.MinLength != 0)
            {
                if (this.APIString.Length < this.MinLength)
                {
                    results.Add(new ValidationResult($"{this.APIStringName} '{this.APIString}' does not meet minimum length requirement: {this.MinLength}."));
                }
            }

            if (this.MaxLength != 0)
            {
                if (this.APIString.Length > this.MaxLength)
                {
                    results.Add(new ValidationResult($"{this.APIStringName} '{this.APIString}' does not meet maximum length requirement: {this.MaxLength}."));
                }
            }

            return results;
        }

        /// <summary>
        /// Equals.
        /// </summary>
        /// <param name="apiString">ApiString.</param>
        /// <returns>Bool.</returns>
        public bool Equals(ApiString apiString)
        {
            if (ReferenceEquals(null, apiString))
            {
                return false;
            }

            if (ReferenceEquals(this, apiString))
            {
                return true;
            }

            return this.APIString == apiString.APIString;
        }

        /// <summary>
        /// Equals.
        /// </summary>
        /// <param name="obj">Object.</param>
        /// <returns>Bool.</returns>
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

            return this.Equals((ApiString)obj);
        }

        /// <summary>
        /// GetHashCode.
        /// </summary>
        /// <returns>Hash Code.</returns>
        public override int GetHashCode()
        {
            return this.APIString != null ? this.APIString.GetHashCode() : 0;
        }

        private void SetDefaults()
        {
            this.APIStringName = "APIString";
            this.APIString = null;
            this.AllowNull = false;
            this.MatchPattern = null;
            this.MinLength = 0;
            this.MaxLength = 0;
        }
    }
}
