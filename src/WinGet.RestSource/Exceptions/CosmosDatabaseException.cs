// -----------------------------------------------------------------------
// <copyright file="CosmosDatabaseException.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Exceptions
{
    using System;
    using Microsoft.WinGet.RestSource.Models;

    /// <summary>
    /// This is for exceptions that occur with the Cosmos Database Client.
    /// </summary>
    public class CosmosDatabaseException : DefaultException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosDatabaseException"/> class.
        /// </summary>
        public CosmosDatabaseException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosDatabaseException"/> class.
        /// </summary>
        /// <param name="internalRestError">Internal Rest Error.</param>
        public CosmosDatabaseException(InternalRestError internalRestError)
            : base(internalRestError)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosDatabaseException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public CosmosDatabaseException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosDatabaseException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="internalRestError">Internal Rest Error.</param>
        public CosmosDatabaseException(string message, InternalRestError internalRestError)
            : base(message, internalRestError)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosDatabaseException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="inner">Inner exception.</param>
        public CosmosDatabaseException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosDatabaseException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="inner">Inner exception.</param>
        /// <param name="internalRestError">Internal Rest Error.</param>
        public CosmosDatabaseException(string message, Exception inner, InternalRestError internalRestError)
            : base(message, inner, internalRestError)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosDatabaseException"/> class.
        /// </summary>
        /// <param name="exception">Exception.</param>
        public CosmosDatabaseException(Exception exception)
            : base(exception)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosDatabaseException"/> class.
        /// </summary>
        /// <param name="exception">Exception.</param>
        /// <param name="internalRestError">Internal Rest Error.</param>
        public CosmosDatabaseException(Exception exception, InternalRestError internalRestError)
            : base(exception, internalRestError)
        {
        }
    }
}
