// -----------------------------------------------------------------------
// <copyright file="ChannelValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.StringValidators
{
    /// <summary>
    /// ChannelValidator.
    /// </summary>
    public class ChannelValidator : ApiStringValidator
    {
        private const bool Nullable = true;
        private const uint Min = 1;
        private const uint Max = 16;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelValidator"/> class.
        /// </summary>
        public ChannelValidator()
        {
            this.AllowNull = Nullable;
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
