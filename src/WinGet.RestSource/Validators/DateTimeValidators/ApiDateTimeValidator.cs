// -----------------------------------------------------------------------
// <copyright file="ApiDateTimeValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.DateTimeValidators
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// ApiDateTimeValidator.
    /// </summary>
    public class ApiDateTimeValidator : ValidationAttribute
    {
        private static string seperator = " ";

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiDateTimeValidator"/> class.
        /// </summary>
        public ApiDateTimeValidator()
            : base()
        {
            this.SetDefaults();
        }

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
        public ValidationResult ValidateApiDateTime(object value, ValidationContext validationContext)
        {
            return this.IsValid(value, validationContext);
        }

        /// <summary>
        /// Verifies if an object is a valid API DateTime based on the parameters.
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

            // Check for datetime type and convert
            if (value.GetType() != typeof(DateTime))
            {
                errorMessage = this.UpdateErrorMessage(errorMessage, $"{validationContext.DisplayName} in {validationContext.ObjectType} must be a DateTime type.");
                return new ValidationResult(errorMessage);
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
        }
    }
}
