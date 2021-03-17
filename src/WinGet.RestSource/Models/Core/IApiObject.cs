// -----------------------------------------------------------------------
// <copyright file="IApiObject.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Core
{
    /// <summary>
    /// API Data Interface.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    public interface IApiObject<T> : IApiData<T>
    {
        /// <summary>
        /// Update Object.
        /// </summary>
        /// <param name="obj">Object.</param>
        void Update(T obj);
    }
}
