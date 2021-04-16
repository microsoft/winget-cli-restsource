// -----------------------------------------------------------------------
// <copyright file="ApiStringValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.StringValidators
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Base String Validation Attribute.
    /// </summary>
    public class ApiStringValidator : ValidationAttribute, IApiStringValidator
    {
        private static string seperator = " ";

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiStringValidator"/> class.
        /// </summary>
        public ApiStringValidator()
        {
            this.SetDefaults();
        }

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

            // Check Pattern
            if (!string.IsNullOrEmpty(this.MatchPattern) && !string.IsNullOrEmpty(apiString))
            {
                if (!Regex.IsMatch(apiString, this.MatchPattern))
                {
                    errorMessage = this.UpdateErrorMessage(errorMessage, $"{validationContext.DisplayName} in {validationContext.ObjectType} must match validation pattern: '{this.MatchPattern}'.");
                }
            }

            // Check Min Length
            if (this.MinLength != uint.MinValue)
            {
                if (apiString.Length < this.MinLength)
                {
                    errorMessage = this.UpdateErrorMessage(errorMessage, $"{validationContext.DisplayName} in {validationContext.ObjectType} does not meet minimum length requirement: {this.MinLength}.");
                }
            }

            // Check Max Length
            if (this.MaxLength != uint.MinValue)
            {
                if (apiString.Length > this.MaxLength)
                {
                    errorMessage = this.UpdateErrorMessage(errorMessage, $"{validationContext.DisplayName} in {validationContext.ObjectType} does not meet maximum length requirement: {this.MaxLength}.");
                }
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
            this.AllowNull = false;
            this.MatchPattern = null;
            this.MinLength = 0;
            this.MaxLength = 0;
        }
    }
}
