// -----------------------------------------------------------------------
// <copyright file="LoggingContext.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WindowsPackageManager.Rest.Diagnostics
{
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Logging context information to uniquely identify a run.
    /// It is a wrapper of a simple string to provide type safeness over the source code.
    /// </summary>
    public class LoggingContext
    {
        private const string TagFormat = "[{0}: {1}] ";

        private string inner = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingContext"/> class.
        /// </summary>
        public LoggingContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingContext"/> class.
        /// </summary>
        /// <param name="other">Other logging context.</param>
        public LoggingContext(LoggingContext other)
        {
            this.inner = other.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingContext"/> class.
        /// </summary>
        /// <param name="loggingContext">Logging context string.</param>
        public LoggingContext(string loggingContext)
        {
            this.inner = loggingContext;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingContext"/> class.
        /// </summary>
        /// <param name="tag">Tag.</param>
        /// <param name="value">Value.</param>
        public LoggingContext(string tag, string value)
        {
            this.Append(tag, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingContext"/> class.
        /// </summary>
        /// <param name="tags">Tags.</param>
        public LoggingContext(Dictionary<string, string> tags)
        {
            this.Append(tags);
        }

        /// <summary>
        /// Appends tags to the logging context.
        /// </summary>
        /// <param name="tag">Tag.</param>
        /// <param name="value">Value.</param>
        public void Append(string tag, string value)
        {
            this.Append(new Dictionary<string, string>()
            {
                { tag, value },
            });
        }

        /// <summary>
        /// Appends tags to the logging context.
        /// </summary>
        /// <param name="tags">Tags.</param>
        public void Append(Dictionary<string, string> tags)
        {
            StringBuilder loggingContextWithTags = new StringBuilder();

            foreach (var tag in tags)
            {
                loggingContextWithTags.Append(string.Format(TagFormat, tag.Key, tag.Value));
            }

            this.inner += loggingContextWithTags.ToString();
        }

        /// <summary>
        /// Overrides ToString. Returns the inner string. Useful for string interpolations.
        /// </summary>
        /// <returns>Logging context string.</returns>
        public override string ToString() => this.inner;
    }
}
