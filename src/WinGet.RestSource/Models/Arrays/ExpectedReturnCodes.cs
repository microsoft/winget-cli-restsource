// -----------------------------------------------------------------------
// <copyright file="ExpectedReturnCodes.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Arrays
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// List of ExpectedReturnCode.
    /// </summary>
    public class ExpectedReturnCodes : ApiArray<Objects.ExpectedReturnCode>
    {
        private const bool Nullable = true;
        private const bool Unique = true;
        private const uint Max = 128;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpectedReturnCodes"/> class.
        /// </summary>
        public ExpectedReturnCodes()
        {
            this.APIArrayName = nameof(ExpectedReturnCodes);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
            this.MaxItems = Max;
        }

        /// <inheritdoc />
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            // Check Base
            results.AddRange(base.Validate(validationContext));

            Dictionary<int, string> keyValuePairs = new Dictionary<int, string>();

            // Check for duplicate installer return codes.
            foreach (var code in this)
            {
                if (!keyValuePairs.ContainsKey(code.InstallerReturnCode))
                {
                    keyValuePairs.Add(code.InstallerReturnCode, code.ReturnResponse);
                }
                else
                {
                    results.Add(new ValidationResult($"{validationContext.DisplayName} in {validationContext.ObjectType} contains duplicate code {code.InstallerReturnCode}."));
                }
            }

            return results;
        }
    }
}
