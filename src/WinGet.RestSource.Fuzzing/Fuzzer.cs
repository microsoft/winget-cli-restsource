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

    /// <summary>
    /// Fuzzer class.
    /// </summary>
    public static class Fuzzer
    {
        /// <summary>
        /// Target fuzzing method.
        /// </summary>
        /// <param name="input">Fuzz test inputs.</param>
        public static void FuzzTest(ReadOnlySpan<byte> input)
        {
            try
            {
                if (input.Length < 4)
                {
                    return;
                }

                using var ms = new MemoryStream(input.ToArray());
                var result = Task.Run(() => Parser.StreamParser<PackageManifest>(ms)).Result;
            }
            catch (AggregateException ae)
            {
                // Getting the result of a Task can throw an Aggregate Exception.
                // Handle json parsing exceptions as these are expected.
                ae.Handle(ex =>
                {
                    return ex is JsonReaderException || ex is JsonWriterException || ex is JsonSerializationException;
                });
            }
        }
    }
}
