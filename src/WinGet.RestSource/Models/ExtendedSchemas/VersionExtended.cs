// -----------------------------------------------------------------------
// <copyright file="VersionExtended.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.ExtendedSchemas
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Models.Core;
    using Microsoft.WinGet.RestSource.Models.Schemas;
    using Newtonsoft.Json;

    /// <summary>
    /// This extends the core version model by nesting the installers model.
    /// </summary>
    public class VersionExtended : Version, IApiObject<VersionExtended>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VersionExtended"/> class.
        /// </summary>
        public VersionExtended()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionExtended"/> class.
        /// </summary>
        /// <param name="versionExtended">Version Core.</param>
        public VersionExtended(VersionExtended versionExtended)
            : base(versionExtended)
        {
            this.Installers = versionExtended.Installers;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionExtended"/> class.
        /// </summary>
        /// <param name="version">Version Core.</param>
        public VersionExtended(Version version)
            : base(version)
        {
            this.Installers = null;
        }

        /// <summary>
        /// Gets or sets Installers.
        /// </summary>
        [JsonProperty(Order = 1)]
        public Installers Installers { get; set; }

        /// <summary>
        /// Gets or sets Installers.
        /// </summary>
        [JsonProperty(Order = 2)]
        public Locales Locales { get; set; }

        /// <summary>
        /// Operator==.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator ==(VersionExtended left, VersionExtended right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operator!=.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator !=(VersionExtended left, VersionExtended right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// This returns the Version representation of this object.
        /// </summary>
        /// <returns>Version Core Model.</returns>
        public Version GetVersion()
        {
            return new Version(this);
        }

        /// <inheritdoc />
        public void Update(VersionExtended obj)
        {
            base.Update(obj);
            this.Installers = obj.Installers;
            this.Locales = obj.Locales;
        }

        /// <inheritdoc />
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            // Base
            results.AddRange(base.Validate(validationContext));

            // Validate Optional Members
            if (this.Installers != null)
            {
                Validator.TryValidateObject(this.Installers, new ValidationContext(this.Installers, null, null), results);
            }

            if (this.Locales != null)
            {
                Validator.TryValidateObject(this.Locales, new ValidationContext(this.Locales, null, null), results);
            }

            // Return Results
            return results;
        }

        /// <inheritdoc />
        public bool Equals(VersionExtended other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return base.Equals(other)
                   && Equals(this.Installers, other.Installers)
                   && Equals(this.Locales, other.Locales);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((VersionExtended)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Installers != null ? this.Installers.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Locales != null ? this.Locales.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
