// -----------------------------------------------------------------------
// <copyright file="Fuzzer.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Fuzzing
{
    using Microsoft.WinGet.RestSource.Utils.Common;
    using Microsoft.WinGet.RestSource.Utils.Models.Schemas;
    using Newtonsoft.Json;

    public static class Fuzzer
    {
        public static void FuzzTest(ReadOnlySpan<byte> input)
        {
            try
            {
                if (input.Length == 0)
                {
                    return;
                }

                using var ms = new MemoryStream(input.ToArray());
                var result = Task.Run(() => Parser.StreamParser<PackageManifest>(ms)).Result;
            }
            catch (JsonException)
            {
                // Possible Json exceptions include JsonReaderException, JsonWriterException, or JsonSerializationException;
                // Ignore expected exceptions as these are normal failures if the input cannot be deserialized.
                return;
            }
        }
    }
}
