// -----------------------------------------------------------------------
// <copyright file="Parser.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
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
            string requestBody = await new StreamReader(stream).ReadToEndAsync();
            T deserializedObject = JsonConvert.DeserializeObject<T>(requestBody);

            if (log != null)
            {
                log.LogInformation("Deserialized Stream:");
                log.LogInformation(JsonConvert.SerializeObject(deserializedObject, Formatting.Indented));
            }

            return deserializedObject;
        }

        /// <summary>
        /// This will encode a string to Base 64.
        /// </summary>
        /// <param name="plainText">Plain Text.</param>
        /// <returns>Encoded data.</returns>
        public static string Base64Encode(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                return null;
            }

            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// This will decode data from Base 64.
        /// </summary>
        /// <param name="base64EncodedData">Encoded data.</param>
        /// <returns>Plain Text.</returns>
        public static string Base64Decode(string base64EncodedData)
        {
            if (string.IsNullOrEmpty(base64EncodedData))
            {
                return null;
            }

            byte[] base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}