// -----------------------------------------------------------------------
// <copyright file="FormatJSON.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Common
{
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Class that contains JSON helpers for printing and exporting data.
    /// </summary>
    public class FormatJSON
    {
        /// <summary>
        /// Formats an object to a custom formatted JSON string.
        /// </summary>
        /// <typeparam name="T">Type must be an object that can be serialized as JSON.</typeparam>
        /// <param name="obj">Generic object that can be serialized as JSON.</param>
        /// <param name="format">Formatting to use.</param>
        /// <param name="nullValueHandling">Null Handling Value.</param>
        /// <param name="log">Logger.</param>
        /// <returns>A formatted JSON String.</returns>
        public static string Custom<T>(T obj, Formatting format, NullValueHandling nullValueHandling, ILogger log = null)
        {
            return Format(obj, format, nullValueHandling, log);
        }

        /// <summary>
        /// Formats an object to an Indented JSON string.
        /// </summary>
        /// <typeparam name="T">Type must be an object that can be serialized as JSON.</typeparam>
        /// <param name="obj">Generic object that can be serialized as JSON.</param>
        /// <param name="log">Logger.</param>
        /// <returns>A formatted JSON String.</returns>
        public static string Indented<T>(T obj, ILogger log = null)
        {
            return Format(obj, Formatting.Indented, NullValueHandling.Ignore, log);
        }

        /// <summary>
        /// Formats an object to a non-formatted JSON string.
        /// </summary>
        /// <typeparam name="T">Type must be an object that can be serialized as JSON.</typeparam>
        /// <param name="obj">Generic object that can be serialized as JSON.</param>
        /// <param name="log">Logger.</param>
        /// <returns>A formatted JSON String.</returns>
        public static string None<T>(T obj, ILogger log = null)
        {
            return Format(obj, Formatting.None, NullValueHandling.Ignore, log);
        }

        private static string Format<T>(T obj, Formatting format, NullValueHandling nullValueHandling, ILogger log = null)
        {
            string output = JsonConvert.SerializeObject(obj, new JsonSerializerSettings { NullValueHandling = nullValueHandling });
            string jsonFormatted = JValue.Parse(output).ToString(format);
            return jsonFormatted;
        }
    }
}
