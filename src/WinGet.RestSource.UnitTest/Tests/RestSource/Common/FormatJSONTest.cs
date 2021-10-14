// -----------------------------------------------------------------------
// <copyright file="FormatJSONTest.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Winget.RestSource.UnitTest.Tests.RestSource.Common
{
    using System.Collections.Generic;
    using Microsoft.WinGet.RestSource.Common;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Format JSON Tests.
    /// </summary>
    public class FormatJSONTest
    {
        private readonly ITestOutputHelper log;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatJSONTest"/> class.
        /// </summary>
        /// <param name="log">ITestOutputHelper.</param>
        public FormatJSONTest(ITestOutputHelper log)
        {
            this.log = log;
        }

        /// <summary>
        /// This verifies that JSON object is formatted and not indented.
        /// </summary>
        [Fact]
        public void None()
        {
            List<string> objectToFormat = new List<string>
            {
                "a",
                "b",
                "c",
            };

            FormatJSON.None(objectToFormat);
            this.log.WriteLine($"Object to Format: {FormatJSON.Indented(objectToFormat)}");

            string expected = @"[""a"",""b"",""c""]";
            string formattedJson = FormatJSON.None(objectToFormat);
            Assert.Equal(expected, formattedJson);
        }
    }
}
