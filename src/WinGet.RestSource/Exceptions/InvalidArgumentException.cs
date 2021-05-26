// -----------------------------------------------------------------------
// <copyright file="InvalidArgumentException.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Exceptions
{
    using System;
    using Microsoft.WinGet.RestSource.Models.Errors;

    /// <summary>
    /// This is for exceptions that occur with the Cosmos Database Client.
    /// </summary>
    public class InvalidArgumentException : DefaultException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidArgumentException"/> class.
        /// </summary>
        public InvalidArgumentException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidArgumentException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public InvalidArgumentException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidArgumentException"/> class.
        /// </summary>
        /// <param name="internalRestError">Internal Rest Error.</param>
        public InvalidArgumentException(InternalRestError internalRestError)
            : base(internalRestError)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidArgumentException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="internalRestError">Internal Rest Error.</param>
        public InvalidArgumentException(string message, InternalRestError internalRestError)
            : base(message, internalRestError)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidArgumentException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="inner">Inner exception.</param>
        public InvalidArgumentException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidArgumentException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="inner">Inner exception.</param>
        /// <param name="internalRestError">Internal Rest Error.</param>
        public InvalidArgumentException(string message, Exception inner, InternalRestError internalRestError)
            : base(message, inner, internalRestError)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidArgumentException"/> class.
        /// </summary>
        /// <param name="exception">Exception.</param>
        public InvalidArgumentException(Exception exception)
            : base(exception)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidArgumentException"/> class.
        /// </summary>
        /// <param name="exception">Exception.</param>
        /// <param name="internalRestError">Internal Rest Error.</param>
        public InvalidArgumentException(Exception exception, InternalRestError internalRestError)
            : base(exception, internalRestError)
        {
        }
    }
}
