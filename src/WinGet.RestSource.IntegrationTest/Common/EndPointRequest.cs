// -----------------------------------------------------------------------
// <copyright file="EndPointRequest.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.IntegrationTest.Common
{
    using Xunit.Abstractions;
    using static Microsoft.WinGet.RestSource.IntegrationTest.Common.TestCollateral;

    /// <summary>
    /// Represents the endpoint request.
    /// </summary>
    public class EndPointRequest : IXunitSerializable
    {
        /// <summary>
        /// Gets or sets the Endpoint url.
        /// </summary>
        public string RelativeUrlPath { get; set; }

        /// <summary>
        /// Gets or sets the Json file name.
        /// </summary>
        public string JsonFileName { get; set; }

        /// <summary>
        /// Gets or sets the test collateral type.
        /// </summary>
        public TestCollateralType TestCollateralType { get; set; }

        /// <inheritdoc/>
        public void Deserialize(IXunitSerializationInfo info)
        {
            this.RelativeUrlPath = info.GetValue<string>(nameof(this.RelativeUrlPath));
            this.JsonFileName = info.GetValue<string>(nameof(this.JsonFileName));
            this.TestCollateralType = info.GetValue<TestCollateralType>(nameof(this.TestCollateralType));
        }

        /// <inheritdoc/>
        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(this.JsonFileName), this.JsonFileName, typeof(string));
            info.AddValue(nameof(this.RelativeUrlPath), this.RelativeUrlPath, typeof(string));
            info.AddValue(nameof(this.TestCollateralType), this.TestCollateralType, typeof(TestCollateralType));
        }
    }
}
