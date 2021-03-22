// -----------------------------------------------------------------------
// <copyright file="VersionsExtended.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.ExtendedSchemas
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// VersionsExtended.
    /// </summary>
    public class VersionsExtended : ApiArray<VersionExtended>
    {
        private const bool Nullable = true;
        private const bool Unique = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionsExtended"/> class.
        /// </summary>
        public VersionsExtended()
        {
            this.SetDefaults();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionsExtended"/> class.
        /// </summary>
        /// <param name="enumerable">enumerable.</param>
        public VersionsExtended(IEnumerable<VersionExtended> enumerable)
            : base(enumerable)
        {
            this.SetDefaults();
        }

        /// <inheritdoc />
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            // Base
            results.AddRange(base.Validate(validationContext));

            // Verify Unique version numbers
            if (this.Select(obj => obj.PackageVersion).Distinct().Count() < this.Count())
            {
                results.Add(new ValidationResult($"{this.APIArrayName} does not meet unique item requirement: {this}."));
            }

            return results;
        }

        private void SetDefaults()
        {
            this.APIArrayName = nameof(VersionsExtended);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
        }
    }
}
