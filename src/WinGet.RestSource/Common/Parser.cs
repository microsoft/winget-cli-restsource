// -----------------------------------------------------------------------
// <copyright file="Parser.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Common
{
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>
    /// This class holds our common parsing functions.
    /// </summary>
    public static class Parser
    {
        /// <summary>
        /// This will convert a stream to an object.
        /// </summary>
        /// <param name="stream">Stream.</param>
        /// <param name="log">Log Interface.</param>
        /// <typeparam name="T">Object to return.</typeparam>
        /// <returns>Object.</returns>
        public static async Task<T> StreamParser<T>(Stream stream, ILogger log = null)
            where T : class
        {
            // TODO: Throw Deserialization Error on failure.
            string requestBody = await new StreamReader(stream).ReadToEndAsync();
            T deserializedObject = StringParser<T>(requestBody);

            return deserializedObject;
        }

        /// <summary>
        /// This will convert a stream to an object.
        /// </summary>
        /// <param name="str">str.</param>
        /// <param name="log">Log Interface.</param>
        /// <typeparam name="T">Object to return.</typeparam>
        /// <returns>Object.</returns>
        public static T StringParser<T>(string str, ILogger log = null)
            where T : class
        {
            // TODO: Throw Deserialization Error on failure.
            T deserializedObject = JsonConvert.DeserializeObject<T>(str);

            if (log != null)
            {
                log.LogInformation("Deserialized Stream:");
                log.LogInformation(JsonConvert.SerializeObject(deserializedObject, Formatting.Indented));
            }

            return deserializedObject;
        }
    }
}
