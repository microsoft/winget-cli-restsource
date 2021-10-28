// -----------------------------------------------------------------------
// <copyright file="IApiStringValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.StringValidators
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// IApiStringValidator.
    /// </summary>
    public interface IApiStringValidator
    {
        /// <summary>
        /// ValidateApiString.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="validationContext">Validation Context.</param>
        /// <returns>ValidationResult.</returns>
        ValidationResult ValidateApiString(object value, ValidationContext validationContext);
    }
}
