// -----------------------------------------------------------------------
// <copyright file="DiagnosticEvent.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WindowsPackageManager.Rest.Diagnostics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Struct defining a Diagnostic Event.
    /// </summary>
    internal struct DiagnosticEvent : IReadOnlyList<KeyValuePair<string, object>>
    {
        /// <summary>
        /// The message.
        /// </summary>
        public string Message;

        /// <summary>
        /// The member name.
        /// </summary>
        public string MemberName;

        /// <summary>
        /// The source file path.
        /// </summary>
        public string SourceFilePath;

        /// <summary>
        /// The source line number.
        /// </summary>
        public int SourceLineNumber;

        /// <summary>
        /// The tracing level.
        /// </summary>
        public string TracingLevel;

        /// <summary>
        /// Gets the formatter function for the Diagnostic Event.
        /// </summary>
        public static Func<DiagnosticEvent, Exception, string> Formatter { get; } = (s, e) => "Diagnostic event logged.";

        /// <summary>
        /// Gets the count.
        /// </summary>
        public int Count => 5;

        /// <inheritdoc/>
        public KeyValuePair<string, object> this[int index] => index switch
        {
            0 => new KeyValuePair<string, object>("Message", this.Message),
            1 => new KeyValuePair<string, object>("MemberName", this.MemberName),
            2 => new KeyValuePair<string, object>("SourceFilePath", this.SourceFilePath),
            3 => new KeyValuePair<string, object>("SourceLineNumber", this.SourceLineNumber),
            4 => new KeyValuePair<string, object>("TracingLevel", this.TracingLevel),
            _ => throw new IndexOutOfRangeException(nameof(index)),
        };

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            for (var i = 0; i < this.Count; i++)
            {
                yield return this[i];
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
