// -----------------------------------------------------------------------
// <copyright file="ApiObjectResultTest.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Winget.RestSource.UnitTest.Tests.Functions.Common
{
    using Microsoft.WinGet.RestSource.Functions.Common;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// API Object Result Test.
    /// </summary>
    public class ApiObjectResultTest
    {
        private ITestOutputHelper log;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiObjectResultTest"/> class.
        /// </summary>
        /// <param name="log">ITestOutputHelper.</param>
        public ApiObjectResultTest(ITestOutputHelper log)
        {
            this.log = log;
        }

        /// <summary>
        /// This verifies that the default object results are set as expected.
        /// </summary>
        [Fact]
        public void Default()
        {
            string objectResult = "objectResult";
            ApiObjectResult apiObjectResult = new ApiObjectResult(objectResult);
            Assert.Equal(200, apiObjectResult.StatusCode);
        }
    }
}
