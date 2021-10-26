// -----------------------------------------------------------------------
// <copyright file="ApiEnumValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.EnumValidators
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Microsoft.WinGet.RestSource.Utils.Common;
    using Microsoft.WinGet.RestSource.Utils.Validators.StringValidators;

    /// <summary>
    /// ApiEnumValidator.
    /// </summary>
    public class ApiEnumValidator : ValidationAttribute, IApiStringValidator
    {
        private static string seperator = " ";

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEnumValidator"/> class.
        /// </summary>
        public ApiEnumValidator()
            : base()
        {
            this.SetDefaults();
        }

        /// <summary>
        /// Gets or sets Values.
        /// </summary>
        protected List<string> Values { get; set; }

        /// <summary>
        /// Gets or sets StringComparer.
        /// </summary>
        protected StringComparer StringComparer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to Allow Null.
        /// </summary>
        protected bool AllowNull { get; set; }

        /// <summary>
        /// Public Facing Validation.
        /// </summary>
        /// <param name="value">Object to validate.</param>
        /// <param name="validationContext">Validation Context.</param>
        /// <returns>Validation Result.</returns>
        public ValidationResult ValidateApiString(object value, ValidationContext validationContext)
        {
            return this.IsValid(value, validationContext);
        }

        /// <summary>
        /// Verifies if an object is a valid API string based on the parameters.
        /// </summary>
        /// <param name="value">Object to validate.</param>
        /// <param name="validationContext">Validation Context.</param>
        /// <returns>Validation Result.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string errorMessage = null;

            // Check for allowed null
            if (this.AllowNull && value == null)
            {
                return ValidationResult.Success;
            }

            // Check for disallowed null
            if (!this.AllowNull && value == null)
            {
                errorMessage = this.UpdateErrorMessage(errorMessage, $"{validationContext.DisplayName} in {validationContext.ObjectType} must not be null.");
                return new ValidationResult(errorMessage);
            }

            // Check for string type and convert
            if (value.GetType() != typeof(string))
            {
                errorMessage = this.UpdateErrorMessage(errorMessage, $"{validationContext.DisplayName} in {validationContext.ObjectType} must be a string.");
                return new ValidationResult(errorMessage);
            }

            string apiString = value.ToString();

            // Check Enum
            if (!this.Values.Contains(apiString, this.StringComparer))
            {
                errorMessage = this.UpdateErrorMessage(errorMessage, $"{validationContext.DisplayName} in {validationContext.ObjectType} did not match an allowable enum value {FormatJSON.None(this.Values)}.");
            }

            // Finalize Validation.
            if (string.IsNullOrEmpty(errorMessage))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(errorMessage);
        }

        private string UpdateErrorMessage(string original, string update)
        {
            return string.Join(seperator, original, update);
        }

        private void SetDefaults()
        {
            this.Values = new List<string>();
            this.AllowNull = false;
            this.StringComparer = StringComparer.Ordinal;
        }
    }
}
