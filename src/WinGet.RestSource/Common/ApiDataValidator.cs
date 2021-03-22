// -----------------------------------------------------------------------
// <copyright file="ApiDataValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Common
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Microsoft.Extensions.Logging;
    using Microsoft.WinGet.RestSource.Constants;
    using Microsoft.WinGet.RestSource.Exceptions;
    using Microsoft.WinGet.RestSource.Models.Errors;

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
        /// <param name="log">Logger.</param>
        /// <typeparam name="T">Validatable Type.</typeparam>
        public static void Validate<T>(IValidatableObject validatable, ILogger log = null)
            where T : IValidatableObject
        {
            var results = new List<ValidationResult>();

            // Validate Parsed Values
            bool valid = Validator.TryValidateObject(validatable, new ValidationContext(validatable, null, null), results);

            if (!valid)
            {
                string error = string.Join(seperator, ErrorConstants.ValidationFailureErrorMessage);
                error = results.Aggregate(error, (current, validationResult) => string.Join(seperator, current, validationResult.ErrorMessage));

                throw new InvalidArgumentException(
                    new InternalRestError(
                        ErrorConstants.ValidationFailureErrorCode,
                        error));
            }
        }
    }
}
