// -----------------------------------------------------------------------
// <copyright file="InvocationParameterValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.StringValidators
{
    /// <summary>
    /// InvocationParameterValidator.
    /// </summary>
    public class InvocationParameterValidator : ApiStringValidator
    {
        private const bool Nullable = true;
        private const uint Max = 2048;
        private const uint Min = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvocationParameterValidator"/> class.
        /// </summary>
        public InvocationParameterValidator()
        {
            this.AllowNull = Nullable;
            this.MaxLength = Max;
            this.MinLength = Min;
        }
    }
}
