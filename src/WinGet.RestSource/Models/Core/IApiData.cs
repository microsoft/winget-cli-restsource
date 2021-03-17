﻿// -----------------------------------------------------------------------
// <copyright file="IApiData.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Core
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
