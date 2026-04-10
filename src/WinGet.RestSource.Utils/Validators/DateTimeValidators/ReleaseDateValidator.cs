// -----------------------------------------------------------------------
// <copyright file="ReleaseDateValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.DateTimeValidators
{
    /// <summary>
    /// ReleaseDateValidator.
    /// </summary>
    public class ReleaseDateValidator : ApiDateTimeValidator
    {
        private const bool Nullable = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReleaseDateValidator"/> class.
        /// </summary>
        public ReleaseDateValidator()
        {
            this.AllowNull = Nullable;
        }
    }
}
