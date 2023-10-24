// -----------------------------------------------------------------------
// <copyright file="DefaultLocalePartials.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.Source.Models.DefaultLocale
{
    using YamlDotNet.Serialization;

    /// <summary>
    /// Partial class that extends the property definitions of Agreement.
    /// </summary>
    public partial class Agreement
    {
        /// <summary>
        /// Gets or sets the agreement text content.
        /// </summary>
        [YamlMember(Alias = "Agreement")]
        [System.ComponentModel.DataAnnotations.StringLength(10000, MinimumLength = 1)]
        public string AgreementContent { get; set; }
    }
}
