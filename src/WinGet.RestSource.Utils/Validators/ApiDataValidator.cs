﻿// -----------------------------------------------------------------------
// <copyright file="ApiDataValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Exceptions;
    using Microsoft.WinGet.RestSource.Utils.Models.Errors;

    /// <summary>
    /// This class holds our common validation process.
    /// </summary>
    public static class ApiDataValidator
    {
        private static string seperator = " ";

        /// <summary>
        /// Performs Common Validation Process.
        /// </summary>
        /// <param name="validatable">Validatable Object.</param>
        /// <typeparam name="T">Validatable Type.</typeparam>
        public static void Validate<T>(T validatable)
        {
            var results = new List<ValidationResult>();

            // Validate Parsed Values
            Validate<T>(validatable, results);

            if (results.Count > 0)
            {
                string error = string.Join(seperator, ErrorConstants.ValidationFailureErrorMessage);
                error = results.Aggregate(error, (current, validationResult) => string.Join(seperator, current, validationResult.ErrorMessage));

                throw new InvalidArgumentException(
                    new InternalRestError(
                        ErrorConstants.ValidationFailureErrorCode,
                        error));
            }
        }

        /// <summary>
        /// Performs Common Validation Process.
        /// </summary>
        /// <param name="validatable">Validatable Object.</param>
        /// <param name="validationResults">Validation Results.</param>
        /// <typeparam name="T">Validatable Type.</typeparam>
        /// <returns>bool.</returns>
        public static List<ValidationResult> Validate<T>(T validatable, List<ValidationResult> validationResults)
        {
            if (validationResults == null)
            {
                validationResults = new List<ValidationResult>();
            }

            Validator.TryValidateObject(validatable, new ValidationContext(validatable, null, null), validationResults, true);

            return validationResults;
        }

        /// <summary>
        /// Asserts an object is not null, and throws a validation error if it is.
        /// </summary>
        /// <param name="obj">Object to Test.</param>
        /// <typeparam name="T">Type of Object.</typeparam>
        public static void NotNull<T>(T obj)
        {
            if (obj == null)
            {
                throw new InvalidArgumentException(
                    new InternalRestError(
                        ErrorConstants.ValidationFailureErrorCode,
                        ErrorConstants.ValidationFailureErrorMessage));
            }
        }

        /// <summary>
        /// Asserts a string is not null, and throws a validation error if it is.
        /// </summary>
        /// <param name="str">String to Test.</param>
        public static void NotNull(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new InvalidArgumentException(
                    new InternalRestError(
                        ErrorConstants.ValidationFailureErrorCode,
                        ErrorConstants.ValidationFailureErrorMessage));
            }
        }
    }
}
