// -----------------------------------------------------------------------
// <copyright file="RetryHelper.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WindowsPackageManager.Rest.Utils
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Msix.Utils.Logger;
    using Microsoft.WindowsPackageManager.Rest.Diagnostics;

    /// <summary>
    /// Class to deal with retries.
    /// </summary>
    public static class RetryHelper
    {
        /// <summary>
        /// Maximum allowed retries.
        /// </summary>
        public const int MaxRetries = 3;

        /// <summary>
        /// Wait time between retries in milliseconds.
        /// </summary>
        public const int WaitTime = 10000;

        /// <summary>
        /// Runs the specified async lambda that returns a value with retry.
        /// </summary>
        /// <typeparam name="TReturn">Task return type.</typeparam>
        /// <typeparam name="TException">Exception for retry.</typeparam>
        /// <param name="func">Function to run.</param>
        /// <param name="name">Name for logging.</param>
        /// <param name="exp">Exception. This is a hack to make the compiler figure out TReturn
        /// without us knowing it :). </param>
        /// <param name="loggingContext">Logging context.</param>
        /// <param name="maxRetries">Max retries.</param>
        /// <param name="waitTime">Wait time in milliseconds.</param>
        /// <returns>TReturn.</returns>
        public static async Task<TReturn> RunAndRetryWithExceptionAsync<TReturn, TException>(
            Func<Task<TReturn>> func,
            string name,
            TException exp,
            LoggingContext loggingContext,
            int maxRetries = MaxRetries,
            int waitTime = WaitTime)
            where TException : Exception
        {
            if (maxRetries <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRetries));
            }

            int tries = 0;
            while (true)
            {
                try
                {
                    Logger.Info($"{loggingContext} Attempting to run: {name}. Try count: {tries}");
                    return await func();
                }
                catch (TException e)
                {
                    Logger.Warning($"{loggingContext} Error occurred while running the callback function {name}. Going to retry. {e}");

                    if (++tries < maxRetries)
                    {
                        Logger.Info($"{loggingContext} Waiting for {waitTime} milliseconds before retrying. {e}");
                        await Task.Delay(waitTime);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Runs the specified lambda with retry.
        /// </summary>
        /// <typeparam name="TException">Exception for retry.</typeparam>
        /// <param name="func">Function to run.</param>
        /// <param name="name">Name for logging.</param>
        /// <param name="loggingContext">Logging context.</param>
        /// <param name="maxRetries">Max retries.</param>
        /// <param name="waitTime">Wait time in milliseconds.</param>
        /// <returns>TReturn.</returns>
        public static async Task RunAndRetryWithExceptionAsync<TException>(
            Func<Task> func,
            string name,
            LoggingContext loggingContext,
            int maxRetries = MaxRetries,
            int waitTime = WaitTime)
            where TException : Exception
        {
            if (maxRetries <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRetries));
            }

            int tries = 0;
            while (true)
            {
                try
                {
                    Logger.Info($"{loggingContext} Attempting to run: {name}. Try count: {tries}");
                    await func();
                    return;
                }
                catch (TException e)
                {
                    Logger.Warning($"{loggingContext} Error occurred while running the callback function {name}. Going to retry. {e}");

                    if (++tries < maxRetries)
                    {
                        Logger.Info($"{loggingContext} Waiting for {waitTime} milliseconds before retrying. {e}");
                        await Task.Delay(waitTime);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Retry when the function returns a specific value.
        /// </summary>
        /// <typeparam name="TReturn">Return type.</typeparam>
        /// <param name="func">Function to call.</param>
        /// <param name="name">Name of the function for logging.</param>
        /// <param name="retryValue">If obtain do the retry.</param>
        /// <param name="loggingContext">Logging Context.</param>
        /// <param name="maxRetries">Max retries to do.</param>
        /// <param name="waitTime">Waiting time in milliseconds.</param>
        /// <returns>TReturn. If the retries expired, then return the retry value.</returns>
        public static async Task<TReturn> RunAndRetryWithValueAsync<TReturn>(
            Func<Task<TReturn>> func,
            string name,
            TReturn retryValue,
            LoggingContext loggingContext,
            int maxRetries = MaxRetries,
            int waitTime = WaitTime)
            where TReturn : struct, Enum
        {
            if (maxRetries <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRetries));
            }

            int tries = 0;
            while (true)
            {
                Logger.Info($"{loggingContext} Attempting to run: {name}. Try count: {tries}");
                TReturn returnValue = await func();

                if (!returnValue.Equals(retryValue))
                {
                    return returnValue;
                }

                if (++tries < maxRetries)
                {
                    Logger.Info($"{loggingContext} Waiting for {waitTime} milliseconds before retrying {name}");
                    await Task.Delay(waitTime);
                }
                else
                {
                    // If we are here we always had the retry value. Returning it.
                    Logger.Warning($"{loggingContext} Finish retries {name}, returning retry value.");
                    return retryValue;
                }
            }
        }
    }
}
