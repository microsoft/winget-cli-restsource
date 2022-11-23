// -----------------------------------------------------------------------
// <copyright file="Installers.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Schemas
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Exceptions;
    using Microsoft.WinGet.RestSource.Utils.Models.Core;
    using Microsoft.WinGet.RestSource.Utils.Models.Errors;
    using Microsoft.WinGet.RestSource.Utils.Validators;

    /// <summary>
    /// Installers.
    /// </summary>
    public class Installers : ApiArray<Installer>
    {
        private const bool Nullable = true;
        private const bool Unique = true;
        private const uint Max = 1024;

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

        /// <summary>
        /// Add new Installer.
        /// </summary>
        /// <param name="obj">Installer to add.</param>
        public new void Add(Installer obj)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(obj);

            // Verify installer does not exist
            this.AssertInstallerDoesNotExists(obj.InstallerIdentifier);

            base.Add(obj);
        }

        /// <summary>
        /// Update an Installer.
        /// </summary>
        /// <param name="obj">New Installer.</param>
        public void Update(Installer obj)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(obj);

            // Verify installer exists
            this.AssertInstallerExists(obj.InstallerIdentifier);

            // Update
            this[this.FindIndex(0, installer => installer.InstallerIdentifier == obj.InstallerIdentifier)].Update(obj);
        }

        /// <summary>
        /// Remove an Installer.
        /// </summary>
        /// <param name="installerIdentifier">Installer Identifier to remove.</param>
        public void Remove(string installerIdentifier)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(installerIdentifier);

            // Verify installer exists
            this.AssertInstallerExists(installerIdentifier);

            this.RemoveAll(installer => installer.InstallerIdentifier == installerIdentifier);
        }

        /// <summary>
        /// Get Installers.
        /// </summary>
        /// <param name="installerIdentifier">Installer Identifier to get.</param>
        /// <returns>Installers.</returns>
        public Installers Get(string installerIdentifier)
        {
            Installers installers = new Installers();

            if (string.IsNullOrWhiteSpace(installerIdentifier))
            {
                installers = this;
            }
            else
            {
                this.AssertInstallerExists(installerIdentifier);
                installers.Add(this[this.FindIndex(0, installer => installer.InstallerIdentifier == installerIdentifier)]);
            }

            return installers;
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
                results.Add(new ValidationResult($"{validationContext.DisplayName} in {validationContext.ObjectType} does not meet unique item requirement."));
            }

            return results;
        }

        private void SetDefaults()
        {
            this.APIArrayName = nameof(Installers);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
            this.MaxItems = Max;
        }

        private bool InstallerExists(string installerIdentifier)
        {
            return this.Any(p => p.InstallerIdentifier == installerIdentifier);
        }

        private void AssertInstallerExists(string installerIdentifier)
        {
            if (!this.InstallerExists(installerIdentifier))
            {
                throw new InvalidArgumentException(
                    new InternalRestError(
                        ErrorConstants.InstallerDoesNotExistErrorCode,
                        ErrorConstants.InstallerDoesNotExistErrorMessage));
            }
        }

        private void AssertInstallerDoesNotExists(string installerIdentifier)
        {
            if (this.InstallerExists(installerIdentifier))
            {
                throw new InvalidArgumentException(
                    new InternalRestError(
                        ErrorConstants.InstallerAlreadyExistsErrorCode,
                        ErrorConstants.InstallerAlreadyExistsErrorMessage));
            }
        }
    }
}
