// -----------------------------------------------------------------------
// <copyright file="Util.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Winget.RestSource.UnitTest.Common
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Test helper utilities.
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// Executes a foreach async operation on an enumerable source in which iterations are run in parallel.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in source.</typeparam>
        /// <param name="source">The enumerable that contains the original data source.</param>
        /// <param name="body">The async delegate that is invoked once per iteration.</param>
        /// <param name="maxDegreeOfParallelism">The maximum number of concurrent tasks.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public static async Task AsyncParallelForEach<TSource>(this IEnumerable<TSource> source, Func<TSource, Task> body, int maxDegreeOfParallelism)
        {
            var semaphore = new Semaphore(maxDegreeOfParallelism, maxDegreeOfParallelism);
            var tasks = new List<Task>();

            foreach (var item in source)
            {
                semaphore.WaitOne();
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        await body(item);
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }

            // Wait until all are done
            await Task.WhenAll(tasks);
        }
    }
}
