// -----------------------------------------------------------------------
// <copyright file="EndHttpMethod.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.IntegrationTest.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Endpoint http method.
    /// </summary>
    public enum EndHttpMethod
    {
        /// <summary>
        /// Post request.
        /// </summary>
        Post,

        /// <summary>
        /// Get request.
        /// </summary>
        Get,
    }
}
