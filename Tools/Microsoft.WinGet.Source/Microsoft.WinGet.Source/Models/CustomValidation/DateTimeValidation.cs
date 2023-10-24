// -----------------------------------------------------------------------
// <copyright file="DateTimeValidation.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.Source.Models.CustomValidation
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Custom validator for the date time strings.
    /// </summary>
    public class DateTimeValidation : ValidationAttribute
    {
        /// <inheritdoc />
        public override bool IsValid(object value)
        {
            return DateTime.TryParse((string)value, out DateTime dateTime);
        }
    }
}
