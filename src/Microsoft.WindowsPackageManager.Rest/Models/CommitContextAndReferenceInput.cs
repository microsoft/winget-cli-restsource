// -----------------------------------------------------------------------
// <copyright file="CommitContextAndReferenceInput.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WindowsPackageManager.Rest.Models
{
    using System;
    using System.Reflection;
    using System.Text;
    using Newtonsoft.Json;

    /// <summary>
    /// Class that contains context and reference elements for Azure function input.
    /// </summary>
    public class CommitContextAndReferenceInput : ContextAndReferenceInput
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommitContextAndReferenceInput"/> class.
        /// </summary>
        /// <param name="operationId">Operation id of the VALIDATION pipeline.</param>
        /// <param name="sasReference">sasReference to the content to be processed.</param>
        /// <param name="commit">Commit.</param>
        /// <param name="referenceType">Type of operation being performed on the reference package.</param>
        [JsonConstructor]
        public CommitContextAndReferenceInput(
            string operationId,
            string sasReference,
            string commit,
            ReferenceType referenceType)
            : base(operationId, sasReference, referenceType)
        {
            this.Commit = commit;
        }

        /// <summary>
        /// Gets a value indicating the operation id of the VALIDATION pipeline that validated this content.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Commit { get; private set; }

        /// <inheritdoc/>
        public override void Validate()
        {
            if (string.IsNullOrWhiteSpace(this.OperationId) ||
                string.IsNullOrWhiteSpace(this.SASReference) ||
                string.IsNullOrWhiteSpace(this.Commit))
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