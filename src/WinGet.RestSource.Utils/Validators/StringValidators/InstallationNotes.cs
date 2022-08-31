// -----------------------------------------------------------------------
// <copyright file="InstallationNotes.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.StringValidators
{
    /// <summary>
    /// ReleaseNotesValidator.
    /// </summary>
    public class InstallationNotes : ApiStringValidator
    {
        private const bool Nullable = true;
        private const uint Max = 256;
        private const uint Min = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallationNotes"/> class.
        /// </summary>
        public InstallationNotes()
        {
            this.AllowNull = Nullable;
            this.MaxLength = Max;
            this.MinLength = Min;
        }
    }
}
