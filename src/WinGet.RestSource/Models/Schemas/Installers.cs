// -----------------------------------------------------------------------
// <copyright file="Installers.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;

namespace Microsoft.WinGet.RestSource.Models.Schemas
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// Installers.
    /// </summary>
    public class Installers : ApiArray<Installer>
    {
        private const bool Nullable = true;
        private const bool Unique = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="Installers"/> class.
        /// </summary>
        public Installers()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Installers"/> class.
        /// </summary>
        /// <param name="enumerable">Enumerable.</param>
        public Installers(IEnumerable<Installer> enumerable)
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
            if (this.Select(obj => obj.InstallerIdentifier).Distinct().Count() < this.Count())
            {
                results.Add(new ValidationResult($"{this.APIArrayName} does not meet unique item requirement: {this}."));
            }

            return results;
        }

        private void SetDefaults()
        {
            this.APIArrayName = nameof(Installers);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
        }
    }
}
