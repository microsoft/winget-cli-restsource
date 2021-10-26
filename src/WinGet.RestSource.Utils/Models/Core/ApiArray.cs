// -----------------------------------------------------------------------
// <copyright file="ApiArray.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Microsoft.WinGet.RestSource.Utils.Validators;
    using Microsoft.WinGet.RestSource.Utils.Validators.StringValidators;

    /// <summary>
    /// API Array.
    /// </summary>
    /// <typeparam name="T">IValidatableObject.</typeparam>
    public class ApiArray<T> : List<T>, IApiData<T>
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
        /// Gets or sets MaxItems.
        /// </summary>
        protected Type MemberValidator { get; set; }

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

        /// <summary>
        /// Make Distinct.
        /// </summary>
        public void MakeDistinct()
        {
            List<T> distinctList = this.Distinct().ToList();
            this.Clear();
            this.AddRange(distinctList);
        }

        /// <inheritdoc />
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Create Validation Results
            List<ValidationResult> results = new List<ValidationResult>();

            // Check if Null and Not Allowed
            if (!this.AllowNull && this.Count == 0)
            {
                results.Add(new ValidationResult($"{validationContext.DisplayName} in {validationContext.ObjectType} must not be empty."));
                return results;
            }

            // Validate All Member Objects
            if (this.MemberValidator != null)
            {
                var d = Activator.CreateInstance(this.MemberValidator) as IApiStringValidator;
                foreach (T member in this)
                {
                    results.Add(d.ValidateApiString(member, validationContext));
                }
            }
            else if (this.ValidateMembers)
            {
                foreach (T member in this)
                {
                    ApiDataValidator.Validate(member, results);
                }
            }

            // Validate Min Items
            if (this.MinItems != 0)
            {
                if (this.Count < this.MinItems)
                {
                    results.Add(new ValidationResult($"{validationContext.DisplayName} in {validationContext.ObjectType} does not meet minimum item requirement: {this.MinItems}."));
                }
            }

            // Validate Max Items
            if (this.MaxItems != 0)
            {
                if (this.Count > this.MaxItems)
                {
                    results.Add(new ValidationResult($"{validationContext.DisplayName} in {validationContext.ObjectType} does not meet maximum item requirement: {this.MaxItems}."));
                }
            }

            // Validate Unique Items
            if (this.UniqueItems)
            {
                if (this.Distinct().Count() < this.Count())
                {
                    results.Add(new ValidationResult($"{validationContext.DisplayName} in {validationContext.ObjectType} does not meet unique item requirement."));
                }
            }

            // Return Results
            return results;
        }

        /// <summary>
        /// Invalid comparison, present only to satisfy IEquality interface.
        /// </summary>
        /// <param name="other">Object to be compared to.</param>
        /// <returns>Will never return.</returns>
        /// <exception cref="NotImplementedException">Will always throw.</exception>
        public bool Equals(T other)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Compare this to another object which inherits List.
        /// </summary>
        /// <param name="other">List object to compare to.</param>
        /// <returns>True if lists contain same items in same order, false otherwise.</returns>
        public bool Equals(List<T> other)
        {
            return this.SequenceEqual(other);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is List<T> apiArray && this.Equals(apiArray);
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
