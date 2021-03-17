// -----------------------------------------------------------------------
// <copyright file="FormatJSON.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Common
{
    using System.IO;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Helper class that contains JSON helpers used by SmartScreen and Manifest verification components.
    /// </summary>
    public class FormatJSON
    {
        /// <summary>
        /// Formats an object to a formatted JSON string.
        /// </summary>
        /// <typeparam name="T">Type must be an object that can be serialized as JSON.</typeparam>
        /// <param name="obj">Generic object that can be serialized as JSON.</param>
        /// <param name="format">Formatting to use.</param>
        /// <param name="nullValueHandling">Null Handling Value.</param>
        /// <param name="log">Logger.</param>
        /// <returns>A formatted JSON String.</returns>
        public static string Custom<T>(T obj, Formatting format, NullValueHandling nullValueHandling, ILogger log = null)
        {
            return Format(obj, Formatting.Indented, NullValueHandling.Ignore, log);
        }

        /// <summary>
        /// Formats an object to a formatted JSON string.
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
        /// Formats an object to a formatted JSON string.
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
