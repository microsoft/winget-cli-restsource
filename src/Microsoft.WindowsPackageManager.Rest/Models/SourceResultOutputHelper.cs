// -----------------------------------------------------------------------
// <copyright file="SourceResultOutputHelper.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WindowsPackageManager.Rest.Models
{
    using System.Reflection;
    using System.Text;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Class that contains the overall source result.
    /// </summary>
    public class SourceResultOutputHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SourceResultOutputHelper"/> class.
        /// </summary>
        /// <param name="overallResult">Overall result for this source operation.</param>
        [JsonConstructor]
        public SourceResultOutputHelper(
            SourceResultType overallResult)
        {
            this.OverallResult = overallResult;
        }

        /// <summary>
        /// Gets a value indicating the overall result of this source operation.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public SourceResultType OverallResult { get; private set; }

        /// <summary>
        /// ToString override.
        /// </summary>
        /// <returns>String with properties and values.</returns>
        public override string ToString()
        {
            PropertyInfo[] propertyInfos = this.GetType().GetProperties();

            var stringBuilder = new StringBuilder();
            foreach (PropertyInfo info in propertyInfos)
            {
                var value = info.GetValue(this, null) ?? "null";

                stringBuilder.Append($"{info.Name} : '{value}' ");
            }

            return stringBuilder.ToString();
        }
    }
}
