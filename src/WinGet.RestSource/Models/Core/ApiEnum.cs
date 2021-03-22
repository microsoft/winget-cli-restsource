// -----------------------------------------------------------------------
// <copyright file="ApiEnum.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Newtonsoft.Json;

    /// <summary>
    /// ApiEnum.
    /// </summary>
    public class ApiEnum : ApiString
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEnum"/> class.
        /// </summary>
        public ApiEnum()
        : base()
        {
            this.SetDefaults();
        }

        /// <summary>
        /// Gets or sets Values.
        /// </summary>
        protected List<string> Values { get; set; }

        /// <summary>
        /// Gets or sets StringComparer.
        /// </summary>
        protected StringComparer StringComparer { get; set; }

        /// <inheritdoc />
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            // Allow Null
            if (this.AllowNull && string.IsNullOrEmpty(this.APIString))
            {
                return results;
            }

            // Base
            results.AddRange(base.Validate(validationContext));

            // Enum
            if (!this.Values.Contains(this.APIString, this.StringComparer))
            {
                results.Add(new ValidationResult($"{this.APIStringName} '{this.APIString}' did not match an allowable enum value {JsonConvert.SerializeObject(this.Values, Formatting.None)}."));
            }

            return results;
        }

        private void SetDefaults()
        {
            this.Values = new List<string>();
            this.StringComparer = StringComparer.Ordinal;
        }
    }
}
