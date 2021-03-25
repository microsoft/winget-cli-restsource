// -----------------------------------------------------------------------
// <copyright file="ApiStringSerializer.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Serializers
{
    using System;
    using Microsoft.WinGet.RestSource.Models.Core;
    using Newtonsoft.Json;

    /// <summary>
    /// API String Serializer.
    /// </summary>
    public class ApiStringSerializer : JsonConverter<ApiString>
    {
        /// <summary>
        /// Write API String JSON.
        /// </summary>
        /// <param name="writer">JSON Writer.</param>
        /// <param name="value">API String.</param>
        /// <param name="serializer">JSON Serializer.</param>
        public override void WriteJson(JsonWriter writer, ApiString value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        /// <summary>
        /// Read API String JSON.
        /// </summary>
        /// <param name="reader">JSON Reader.</param>
        /// <param name="objectType">Object Type.</param>
        /// <param name="existingValue">Existing Value.</param>
        /// <param name="hasExistingValue">Has Existing Value.</param>
        /// <param name="serializer">JSON Serializer.</param>
        /// <returns>ApiString.</returns>
        public override ApiString ReadJson(JsonReader reader, Type objectType, ApiString existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            // Read APIString value.
            string value = (string)reader.Value;

            // If value is null return null.
            if (value == null)
            {
                return null;
            }

            // Create dynamic object, set value, and return
            dynamic d = Activator.CreateInstance(objectType);
            d.SetString(value);
            return d;
        }
    }
}
