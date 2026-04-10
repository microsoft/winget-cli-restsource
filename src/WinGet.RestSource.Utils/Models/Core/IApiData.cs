// -----------------------------------------------------------------------
// <copyright file="IApiData.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Core
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// API Data Interface.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    public interface IApiData<T> : IEquatable<T>, IValidatableObject
    {
    }
}
