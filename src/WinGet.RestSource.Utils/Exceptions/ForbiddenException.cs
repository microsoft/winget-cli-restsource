// -----------------------------------------------------------------------
// <copyright file="ForbiddenException.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Exceptions
{
    using System;
    using Microsoft.WinGet.RestSource.Utils.Models.Errors;

    /// <summary>
    /// This is for exceptions that occur when validating client certificates.
    /// </summary>
    public class ForbiddenException : DefaultException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenException"/> class.
        /// </summary>
        public ForbiddenException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public ForbiddenException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenException"/> class.
        /// </summary>
        /// <param name="internalRestError">Internal Rest Error.</param>
        public ForbiddenException(InternalRestError internalRestError)
            : base(internalRestError)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="internalRestError">Internal Rest Error.</param>
        public ForbiddenException(string message, InternalRestError internalRestError)
            : base(message, internalRestError)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="inner">Inner exception.</param>
        public ForbiddenException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="inner">Inner exception.</param>
        /// <param name="internalRestError">Internal Rest Error.</param>
        public ForbiddenException(string message, Exception inner, InternalRestError internalRestError)
            : base(message, inner, internalRestError)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenException"/> class.
        /// </summary>
        /// <param name="exception">Exception.</param>
        public ForbiddenException(Exception exception)
            : base(exception)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenException"/> class.
        /// </summary>
        /// <param name="exception">Exception.</param>
        /// <param name="internalRestError">Internal Rest Error.</param>
        public ForbiddenException(Exception exception, InternalRestError internalRestError)
            : base(exception, internalRestError)
        {
        }
    }
}
