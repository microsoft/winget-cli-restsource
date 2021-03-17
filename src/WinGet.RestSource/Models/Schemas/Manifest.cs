// -----------------------------------------------------------------------
// <copyright file="Manifest.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Schemas
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Models.Core;
    using Microsoft.WinGet.RestSource.Models.ExtendedSchemas;
    using Newtonsoft.Json;

    /// <summary>
    /// This is a manifest, which is an extension of the package core model, and the extended version model.
    /// </summary>
    public class Manifest : Package, IApiObject<Manifest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Manifest"/> class.
        /// </summary>
        public Manifest()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Manifest"/> class.
        /// </summary>
        /// <param name="manifest">Package Core.</param>
        public Manifest(Manifest manifest)
            : base(manifest)
        {
            this.Versions = manifest.Versions;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Manifest"/> class.
        /// </summary>
        /// <param name="package">Package Core.</param>
        public Manifest(Package package)
            : base(package)
        {
            this.Versions = null;
        }

        /// <summary>
        /// Gets or sets versions.
        /// </summary>
        [JsonProperty(Order = 1)]
        public VersionsExtended Versions { get; set; }

        /// <summary>
        /// Converts to a Package Core.
        /// </summary>
        /// <returns>Package Core.</returns>
        public Package ToPackage()
        {
            Package pc = new Package(this);
            return pc;
        }

        /// <summary>
        /// This updates a Manifest to update match another Manifest.
        /// </summary>
        /// <param name="manifest">Manifest.</param>
        public void Update(Manifest manifest)
        {
            this.Update(manifest.ToPackage());
        }

        /// <inheritdoc />
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            // Base
            results.AddRange(base.Validate(validationContext));

            // Validate Optional Members
            if (this.Versions != null)
            {
                Validator.TryValidateObject(this.Versions, new ValidationContext(this.Versions, null, null), results);
            }

            // Return Results
            return results;
        }

        /// <inheritdoc />
        public bool Equals(Manifest other)
        {
            return base.Equals(other);
        }
    }
}
