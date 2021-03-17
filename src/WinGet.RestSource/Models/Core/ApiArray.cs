// -----------------------------------------------------------------------
// <copyright file="ApiArray.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Core
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    /// <summary>
    /// API Array.
    /// </summary>
    /// <typeparam name="T">IValidatableObject.</typeparam>
    public class ApiArray<T> : List<T>,  IApiData<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiArray{T}"/> class.
        /// </summary>
        public ApiArray()
            : base()
        {
            this.SetDefaults();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiArray{T}"/> class.
        /// </summary>
        /// <param name="enumerable">Enumerable.</param>
        public ApiArray(IEnumerable<T> enumerable)
        : base(enumerable)
        {
            this.SetDefaults();
        }

        /// <summary>
        /// Gets or sets APIArrayName.
        /// </summary>
        public string APIArrayName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to AllowNull.
        /// </summary>
        protected bool AllowNull { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to UniqueItems.
        /// </summary>
        protected bool UniqueItems { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to ValidateMembers.
        /// </summary>
        protected bool ValidateMembers { get; set; }

        /// <summary>
        /// Gets or sets MinItems.
        /// </summary>
        protected uint MinItems { get; set; }

        /// <summary>
        /// Gets or sets MaxItems.
        /// </summary>
        protected uint MaxItems { get; set; }

        /// <summary>
        /// Operator==.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator ==(ApiArray<T> left, ApiArray<T> right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operator!=.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator !=(ApiArray<T> left, ApiArray<T> right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc />
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Create Validation Results
            List<ValidationResult> results = new List<ValidationResult>();

            // Check if Null and Not Allowed
            if (!this.AllowNull && this.Count == 0)
            {
                results.Add(new ValidationResult($"{this.APIArrayName} must not be empty."));
                return results;
            }

            // Validate All Member Objects
            if (this.ValidateMembers)
            {
                foreach (T member in this)
                {
                    Validator.TryValidateObject(member, new ValidationContext(member, null, null), results);
                }
            }

            // Validate Min Items
            if (this.MinItems != 0)
            {
                if (this.Count < this.MinItems)
                {
                    results.Add(new ValidationResult($"{this.APIArrayName} does not meet minimum item requirement: {this.MinItems}."));
                }
            }

            // Validate Max Items
            if (this.MaxItems != 0)
            {
                if (this.Count > this.MaxItems)
                {
                    results.Add(new ValidationResult($"{this.APIArrayName} does not meet maximum item requirement: {this.MaxItems}."));
                }
            }

            // Validate Unique Items
            if (this.UniqueItems)
            {
                if (this.Distinct().Count() < this.Count())
                {
                    results.Add(new ValidationResult($"{this.APIArrayName} does not meet unique item requirement: {this}."));
                }
            }

            // Return Results
            return results;
        }

        /// <inheritdoc />
        public bool Equals(T other)
        {
            if (other == null)
            {
                return false;
            }

            return base.Equals(other);
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

            return this.Equals((ApiArray<T>)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        private void SetDefaults()
        {
            this.AllowNull = false;
            this.UniqueItems = false;
            this.ValidateMembers = true;
            this.MinItems = 0;
            this.MaxItems = 0;
        }
    }
}
