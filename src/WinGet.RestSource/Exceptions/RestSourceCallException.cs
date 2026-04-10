// -----------------------------------------------------------------------
// <copyright file="RestSourceCallException.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Exceptions
{
    using System;
    using System.Net;
    using System.Net.Http;

    /// <summary>
    /// RestSourceCallException.
    /// </summary>
    public class RestSourceCallException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RestSourceCallException"/> class.
        /// </summary>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="httpMethod">HttpMethod.</param>
        /// <param name="httpStatusCode">Http status code.</param>
        public RestSourceCallException(string endpoint, HttpMethod httpMethod, HttpStatusCode httpStatusCode)
            : this($"Endpoint '{endpoint}' HttpMethod '{httpMethod}' StatusCode '{httpStatusCode}'")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestSourceCallException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public RestSourceCallException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestSourceCallException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="inner">Inner exception.</param>
        public RestSourceCallException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
