// -----------------------------------------------------------------------
// <copyright file="ExpectedReturnCode.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Objects
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Constants;
    using Microsoft.WinGet.RestSource.Models.Arrays;
    using Microsoft.WinGet.RestSource.Models.Core;
    using Microsoft.WinGet.RestSource.Validators.EnumValidators;

    /// <summary>
    /// ExpectedReturnCode.
    /// </summary>
    public class ExpectedReturnCode : IApiObject<ExpectedReturnCode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpectedReturnCode"/> class.
        /// </summary>
        public ExpectedReturnCode()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpectedReturnCode"/> class.
        /// </summary>
        /// <param name="expectedReturnCode">Expected return code.</param>
        public ExpectedReturnCode(ExpectedReturnCode expectedReturnCode)
        {
            this.Update(expectedReturnCode);
        }

        /// <summary>
        /// Gets or sets InstallerReturnCode.
        /// </summary>
        public long InstallerReturnCode { get; set; }

        /// <summary>
        /// Gets or sets ReturnResponse.
        /// </summary>
        [ExpectedReturnCodeResponseValidator]
        public string ReturnResponse { get; set; }

        /// <summary>
        /// Operator==.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator ==(ExpectedReturnCode left, ExpectedReturnCode right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operator!=.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator !=(ExpectedReturnCode left, ExpectedReturnCode right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// This updates the current expected return code to match another.
        /// </summary>
        /// <param name="expectedReturnCode">Expected return code.</param>
        public void Update(ExpectedReturnCode expectedReturnCode)
        {
            this.InstallerReturnCode = expectedReturnCode.InstallerReturnCode;
            this.ReturnResponse = expectedReturnCode.ReturnResponse;
        }

        /// <inheritdoc/>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            // Not Zero
            if (this.InstallerReturnCode == 0)
            {
                results.Add(new ValidationResult($"{validationContext.DisplayName} in {validationContext.ObjectType} may not contain an installer return code of 0."));
            }

            // Check bounds
            if (this.InstallerReturnCode > InstallerSuccessCodes.MaximumValue || this.InstallerReturnCode < InstallerSuccessCodes.MinimumValue)
            {
                results.Add(new ValidationResult($"{validationContext.DisplayName} in {validationContext.ObjectType} may not contain an installer return" +
                    $" code that is out of allowable bounds [{InstallerSuccessCodes.MinimumValue}, {InstallerSuccessCodes.MaximumValue}]."));
            }

            return results;
        }

        /// <inheritdoc />
        public bool Equals(ExpectedReturnCode other)
        {
            return (this.InstallerReturnCode, this.ReturnResponse) ==
                   (other.InstallerReturnCode, other.ReturnResponse);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is ExpectedReturnCode expectedReturnCode && this.Equals(expectedReturnCode);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return System.HashCode.Combine(this.InstallerReturnCode, this.ReturnResponse);
        }
    }
}
