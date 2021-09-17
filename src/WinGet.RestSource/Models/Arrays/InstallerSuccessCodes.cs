// -----------------------------------------------------------------------
// <copyright file="InstallerSuccessCodes.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Arrays
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// InstallerSuccessCodes.
    /// </summary>
    public class InstallerSuccessCodes : ApiArray<long>
    {
        /// <summary>
        /// Maximum value of installer return code.
        /// </summary>
        public const long MaximumValue = 4294967295;

        /// <summary>
        /// Minimum value of installer return code.
        /// </summary>
        public const long MinimumValue = -2147483648;

        private const bool Nullable = true;
        private const bool Unique = true;
        private const bool MemberValidation = false;
        private const uint Max = 16;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallerSuccessCodes"/> class.
        /// </summary>
        public InstallerSuccessCodes()
        {
            this.APIArrayName = nameof(InstallerSuccessCodes);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
            this.ValidateMembers = MemberValidation;
            this.MaxItems = Max;
        }

        /// <inheritdoc />
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            // Check Base
            results.AddRange(base.Validate(validationContext));

            // Not Zero
            if (this.Contains(0))
            {
                results.Add(new ValidationResult($"{validationContext.DisplayName} in {validationContext.ObjectType} may not contain an installer success code of 0."));
            }

            // Check bounds
            foreach (var code in this)
            {
                if (code > MaximumValue || code < MinimumValue)
                {
                    results.Add(new ValidationResult($"{validationContext.DisplayName} in {validationContext.ObjectType} contains {code} code " +
                        $"is out of allowable bounds [{InstallerSuccessCodes.MinimumValue}, {InstallerSuccessCodes.MaximumValue}]."));
                }
            }

            return results;
        }
    }
}
