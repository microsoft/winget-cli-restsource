// -----------------------------------------------------------------------
// <copyright file="IFunctionInput.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WindowsPackageManager.Rest.Interfaces
{
    /// <summary>
    /// Generic function input interface.
    /// </summary>
    public interface IFunctionInput
    {
        /// <summary>
        /// Validates all required properties are set.
        /// </summary>
        void Validate();
    }
}
