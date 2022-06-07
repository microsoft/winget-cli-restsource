// -----------------------------------------------------------------------
// <copyright file="ContextAndReferenceInput.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WindowsPackageManager.Rest.Models
{
    using System;
    using System.Reflection;
    using System.Text;
    using Microsoft.WindowsPackageManager.Rest.Interfaces;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Class that contains context and reference elements for Azure function input.
    /// </summary>
    public class ContextAndReferenceInput : IFunctionInput
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContextAndReferenceInput"/> class.
        /// </summary>
        /// <param name="operationId">Operation id of the VALIDATION pipeline that validated this commit.</param>
        /// <param name="sasReference">sasReference to the content to be processed.</param>
        /// <param name="referenceType">Type of operation being performed on the reference package.</param>
        [JsonConstructor]
        public ContextAndReferenceInput(
            string operationId,
            string sasReference,
            ReferenceType referenceType)
        {
            this.OperationId = operationId;
            this.SASReference = sasReference;
            this.ReferenceType = referenceType;
        }

        /// <summary>
        /// Gets a value indicating the operation id of the VALIDATION pipeline that validated this content.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string OperationId { get; private set; }

        /// <summary>
        /// Gets a value indicating a sasReference to the content to be processed.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string SASReference { get; private set; }

        /// <summary>
        /// Gets a value indicating the reference type of the sasReference.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public ReferenceType ReferenceType { get; private set; }

        /// <inheritdoc/>
        public virtual void Validate()
        {
            if (string.IsNullOrWhiteSpace(this.OperationId) ||
                string.IsNullOrWhiteSpace(this.SASReference))
            {
                throw new ArgumentNullException("ContextAndReferenceInputHelper input is missing required fields.");
            }
        }

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

                // Skip logging secrets.
                if (!string.Equals(info.Name, nameof(this.SASReference)))
                {
                    stringBuilder.Append($"{info.Name} : '{value}' ");
                }
            }

            return stringBuilder.ToString();
        }
    }
}