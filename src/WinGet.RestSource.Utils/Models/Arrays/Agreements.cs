// -----------------------------------------------------------------------
// <copyright file="Agreements.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Arrays
{
    using System;
    using Microsoft.WinGet.RestSource.Utils.Models.Core;
    using Microsoft.WinGet.RestSource.Utils.Validators.StringValidators;

    /// <summary>
    /// Agreements.
    /// </summary>
    public class Agreements : ApiArray<Objects.SourceAgreement>
    {
        private const bool Nullable = true;
        private const bool Unique = true;
        private const uint Max = 16;

        /// <summary>
        /// Initializes a new instance of the <see cref="Agreements"/> class.
        /// </summary>
        public Agreements()
        {
            this.APIArrayName = nameof(Agreements);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
            this.MaxItems = Max;
        }
    }
}
