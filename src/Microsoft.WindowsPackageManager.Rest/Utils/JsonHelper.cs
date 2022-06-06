// -----------------------------------------------------------------------
// <copyright file="JsonHelper.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WindowsPackageManager.Rest.Utils
{
    using System.IO;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Helper class for json serialization.
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// Serializes an object to an indented json string.
        /// </summary>
        /// <typeparam name="T">Type must be an object that can be serialized as json.</typeparam>
        /// <param name="obj">Generic object that can be serialized as json.</param>
        /// <returns>A formated json String.</returns>
        public static string SerializeObjectToIndentedJson<T>(T obj)
        {
            return SerializeObject(obj, Formatting.Indented);
        }

        /// <summary>
        /// Serialize an object to a json string.
        /// </summary>
        /// <typeparam name="T">Type must be an object that can be serialized as JSON.</typeparam>
        /// <param name="obj">Generic object that can be serialized as JSON.</param>
        /// <returns>A formated JSON String.</returns>
        public static string SerializeObject<T>(T obj)
        {
            return SerializeObject(obj, Formatting.None);
        }

        /// <summary>
        /// Deserialize a json file to an object.
        /// </summary>
        /// <typeparam name="T">Type must be an object that can be deserialized.</typeparam>
        /// <param name="fileName">Json file path.</param>
        /// <returns>Object corresponding to the json file.</returns>
        public static T DeserializeJsonFileToObject<T>(string fileName)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(fileName));
        }

        private static string SerializeObject<T>(T obj, Formatting format)
        {
            string output = JsonConvert.SerializeObject(obj, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            string jsonFormatted = JValue.Parse(output).ToString(format);
            return jsonFormatted;
        }
    }
}
