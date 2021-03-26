// -----------------------------------------------------------------------
// <copyright file="ApiObjectResult.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Functions.Common
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Default OK Object Result.
    /// </summary>
    public class ApiObjectResult : OkObjectResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiObjectResult"/> class.
        /// </summary>
        /// <param name="value">Object.</param>
        /// <param name="statusCode">Status Code.</param>
        /// <param name="nullValueHandling">Null Handling.</param>
        /// <param name="formatting">Formatting.</param>
        public ApiObjectResult(
            object value,
            int statusCode = 200,
            NullValueHandling nullValueHandling = NullValueHandling.Ignore,
            Formatting formatting = Formatting.None)
            : base(value)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver(),
                NullValueHandling = nullValueHandling,
                Formatting = formatting,
            };
            this.Formatters = new FormatterCollection<IOutputFormatter>();
            this.Formatters.Add(new NewtonsoftJsonOutputFormatter(settings, System.Buffers.ArrayPool<char>.Shared, new MvcOptions()));
            this.StatusCode = statusCode;
        }
    }
}
