﻿// -----------------------------------------------------------------------
// <copyright file="IApiStringValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.StringValidators
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
