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
            using var ms = new MemoryStream(input.ToArray());
            FuzzTestOne<PackageManifest>(ms);
            FuzzTestOne<Installer>(ms);
            FuzzTestOne<Locale>(ms);
            FuzzTestOne<ManifestSearchRequest>(ms);
            FuzzTestOne<Package>(ms);
            FuzzTestOne<Version>(ms);
        }

        private static void FuzzTestOne<T>(MemoryStream ms)
            where T : class
        {
            try
            {
                var result = Task.Run(() => Parser.StreamParser<T>(ms)).Result;
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
