// -----------------------------------------------------------------------
// <copyright file="InstallerSuccessCodes.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
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
    public class InstallerSuccessCodes : ApiArray<int>
    {
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

            return results;
        }
    }
}
