// -----------------------------------------------------------------------
// <copyright file="Locales.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Schemas
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// Locales.
    /// </summary>
    public class Locales : ApiArray<Locale>
    {
        private const bool Nullable = true;
        private const bool Unique = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="Locales"/> class.
        /// </summary>
        public Locales()
        {
            this.SetDefaults();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Locales"/> class.
        /// </summary>
        /// <param name="enumerable">Enumerable.</param>
        public Locales(IEnumerable<Locale> enumerable)
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
            if (this.Select(obj => obj.PackageLocale).Distinct().Count() < this.Count())
            {
                results.Add(new ValidationResult($"{this.APIArrayName} does not meet unique item requirement: {this}."));
            }

            return results;
        }

        private void SetDefaults()
        {
            this.APIArrayName = nameof(Locales);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
        }
    }
}
