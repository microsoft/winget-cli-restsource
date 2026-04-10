// -----------------------------------------------------------------------
// <copyright file="JSONHelpersTests.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Winget.RestSource.UnitTest.Tests.WindowsPackageManager.Rest.Utils
{
    using System.Collections.Generic;
    using System.IO;
    using Microsoft.WindowsPackageManager.Rest.Utils;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// JsonHelper Tests.
    /// </summary>
    public class JSONHelpersTests
    {
        private ITestOutputHelper log;

        /// <summary>
        /// Initializes a new instance of the <see cref="JSONHelpersTests"/> class.
        /// </summary>
        /// <param name="log">Output Helper.</param>
        public JSONHelpersTests(ITestOutputHelper log)
        {
            this.log = log;
        }

        /// <summary>
        /// Test serializing an object.
        /// </summary>
        [Fact]
        public void SerializeObject_Test()
        {
            List<string> objectToFormat = new List<string>
            {
                "a",
                "b",
                "c",
            };

            string expected = @"[""a"",""b"",""c""]";
            string formattedJson = JsonHelper.SerializeObject(objectToFormat);

            Assert.Equal(expected, formattedJson);
        }

        /// <summary>
        /// Test serializing and indenting an object.
        /// </summary>
        [Fact]
        public void SerializeObjectToIndentedJson_Test()
        {
            List<string> objectToFormat = new List<string>
            {
                "a",
                "b",
                "c",
            };

            string expected = "[\r\n  \"a\",\r\n  \"b\",\r\n  \"c\"\r\n]";
            string formattedJson = JsonHelper.SerializeObjectToIndentedJson(objectToFormat);

            Assert.Equal(expected, formattedJson);
        }

        /// <summary>
        /// Test getting the deserialize object from a file.
        /// </summary>
        [Fact]
        public void DeserializeJsonFileToObject_Test()
        {
            List<string> objectToFormat = new List<string>
            {
                "a",
                "b",
                "c",
            };

            string tmpFile = Path.GetTempFileName();
            File.WriteAllText(tmpFile, JsonHelper.SerializeObject(objectToFormat));

            _ = JsonHelper.DeserializeJsonFileToObject<List<string>>(tmpFile);

            File.Delete(tmpFile);
        }
    }
}
