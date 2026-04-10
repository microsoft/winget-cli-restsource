// -----------------------------------------------------------------------
// <copyright file="RetryHelperTests.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Winget.RestSource.UnitTest.Tests.WindowsPackageManager.Rest.Utils
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.WindowsPackageManager.Rest.Diagnostics;
    using Microsoft.WindowsPackageManager.Rest.Utils;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Unit tests for the retry helper.
    /// </summary>
    public class RetryHelperTests
    {
        private const int NeverSucceed = -1;
        private const int SucceedAtFirst = 1;
        private const int SucceedAtThird = 3;
        private const int Fail = -2;

        private readonly ITestOutputHelper log;

        private int numOfCalls = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="RetryHelperTests"/> class.
        /// </summary>
        /// <param name="log"><see cref="ITestOutputHelper"/>.</param>
        public RetryHelperTests(ITestOutputHelper log)
        {
            this.log = log;
        }

        private enum TestEnum
        {
            RetryValue,
            SucceedAtFirst,
            SucceedAtThird,
            Fail,
        }

        /// <summary>
        /// Test method throw bad arguments RunAndRetryWithExceptionAsync.
        /// </summary>
        [Fact]
        public async void RetryHelperException_BadArguments()
        {
            LoggingContext loggingContext = new LoggingContext(nameof(this.RetryHelperException_BadArguments));
            this.numOfCalls = 0;
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                async () => await RetryHelper.RunAndRetryWithExceptionAsync(
                                async () => { return await this.DoSomethingExceptionAsync(NeverSucceed); },
                                nameof(this.RetryHelperException_BadArguments),
                                new ArgumentException(),
                                loggingContext,
                                maxRetries: 0));
            Assert.Equal(0, this.numOfCalls);
        }

        /// <summary>
        /// Test method that never succeeds RunAndRetryWithExceptionAsync.
        /// </summary>
        [Fact]
        public async void RetryHelperException_NeverSucceeds()
        {
            LoggingContext loggingContext = new LoggingContext(nameof(this.RetryHelperException_NeverSucceeds));
            this.numOfCalls = 0;
            await Assert.ThrowsAsync<ArgumentException>(
                async () => await RetryHelper.RunAndRetryWithExceptionAsync(
                                async () => { return await this.DoSomethingExceptionAsync(NeverSucceed); },
                                nameof(this.RetryHelperException_NeverSucceeds),
                                new ArgumentException(),
                                loggingContext));
            Assert.Equal(RetryHelper.MaxRetries, this.numOfCalls);
        }

        /// <summary>
        /// Test method succeeds first call RunAndRetryWithExceptionAsync.
        /// </summary>
        [Fact]
        public async void RetryHelperException_SucceedsAtFirst()
        {
            LoggingContext loggingContext = new LoggingContext(nameof(this.RetryHelperException_SucceedsAtFirst));
            this.numOfCalls = 0;
            var r = await RetryHelper.RunAndRetryWithExceptionAsync(
                async () => { return await this.DoSomethingExceptionAsync(SucceedAtFirst); },
                nameof(this.RetryHelperException_SucceedsAtFirst),
                new ArgumentException(),
                loggingContext);
            Assert.Equal(SucceedAtFirst, this.numOfCalls);
            Assert.IsType<int>(r);
        }

        /// <summary>
        /// Test method succeeds third call RunAndRetryWithExceptionAsync.
        /// </summary>
        [Fact]
        public async void RetryHelperException_SucceedsAtThird()
        {
            LoggingContext loggingContext = new LoggingContext(nameof(this.RetryHelperException_SucceedsAtThird));
            this.numOfCalls = 0;
            var r = await RetryHelper.RunAndRetryWithExceptionAsync(
                async () => { return await this.DoSomethingExceptionAsync(SucceedAtThird); },
                nameof(this.RetryHelperException_SucceedsAtThird),
                new ArgumentException(),
                loggingContext);
            Assert.Equal(SucceedAtThird, this.numOfCalls);
            Assert.IsType<int>(r);
        }

        /// <summary>
        /// Test method fail with another non retry exception RunAndRetryWithExceptionAsync.
        /// </summary>
        [Fact]
        public async void RetryHelperException_FailMiserably()
        {
            LoggingContext loggingContext = new LoggingContext(nameof(this.RetryHelperException_FailMiserably));
            this.numOfCalls = 0;
            await Assert.ThrowsAsync<Exception>(
                async () => await RetryHelper.RunAndRetryWithExceptionAsync(
                                async () => { return await this.DoSomethingExceptionAsync(Fail); },
                                nameof(this.RetryHelperException_FailMiserably),
                                new ArgumentException(),
                                loggingContext));
            Assert.Equal(1, this.numOfCalls);
        }

        /// <summary>
        /// Test method throw bad arguments RunAndRetryWithValueAsync.
        /// </summary>
        [Fact]
        public async void RetryHelperValue_BadArguments()
        {
            LoggingContext loggingContext = new LoggingContext(nameof(this.RetryHelperValue_BadArguments));
            this.numOfCalls = 0;

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                async () => await RetryHelper.RunAndRetryWithValueAsync(
                                async () => { return await this.DoSomethingTestEnumValueAsync(TestEnum.Fail); },
                                nameof(this.RetryHelperValue_BadArguments),
                                TestEnum.RetryValue,
                                loggingContext,
                                maxRetries: 0));
            Assert.Equal(0, this.numOfCalls);
        }

        /// <summary>
        /// Test method that never succeeds RunAndRetryWithValueAsync.
        /// </summary>
        [Fact]
        public async void RetryHelperValue_NeverSucceeds()
        {
            LoggingContext loggingContext = new LoggingContext(nameof(this.RetryHelperValue_NeverSucceeds));
            this.numOfCalls = 0;
            var r = await RetryHelper.RunAndRetryWithValueAsync(
                async () => { return await this.DoSomethingTestEnumValueAsync(TestEnum.RetryValue); },
                nameof(this.RetryHelperValue_NeverSucceeds),
                TestEnum.RetryValue,
                loggingContext);
            Assert.Equal(RetryHelper.MaxRetries, this.numOfCalls);
            Assert.Equal(TestEnum.RetryValue, r);
        }

        /// <summary>
        /// Test method succeeds first call RunAndRetryWithValueAsync.
        /// </summary>
        [Fact]
        public async void RetryHelperValue_SucceedsAtFirst()
        {
            LoggingContext loggingContext = new LoggingContext(nameof(this.RetryHelperValue_SucceedsAtFirst));
            this.numOfCalls = 0;
            var r = await RetryHelper.RunAndRetryWithValueAsync(
                async () => { return await this.DoSomethingTestEnumValueAsync(TestEnum.SucceedAtFirst); },
                nameof(this.RetryHelperValue_SucceedsAtFirst),
                TestEnum.RetryValue,
                loggingContext);
            Assert.Equal(SucceedAtFirst, this.numOfCalls);
            Assert.Equal(TestEnum.SucceedAtFirst, r);
        }

        /// <summary>
        /// Test method succeeds third call RunAndRetryWithValueAsync.
        /// </summary>
        [Fact]
        public async void RetryHelperValue_SucceedsAtThird()
        {
            LoggingContext loggingContext = new LoggingContext(nameof(this.RetryHelperValue_SucceedsAtThird));
            this.numOfCalls = 0;
            var r = await RetryHelper.RunAndRetryWithValueAsync(
                async () => { return await this.DoSomethingTestEnumValueAsync(TestEnum.SucceedAtThird); },
                nameof(this.RetryHelperValue_SucceedsAtThird),
                TestEnum.RetryValue,
                loggingContext);
            Assert.Equal(SucceedAtThird, this.numOfCalls);
            Assert.Equal(TestEnum.SucceedAtThird, r);
        }

        /// <summary>
        /// Test method fail with another non retry exception RunAndRetryWithValueAsync.
        /// </summary>
        [Fact]
        public async void RetryHelperValue_FailMiserably()
        {
            LoggingContext loggingContext = new LoggingContext(nameof(this.RetryHelperValue_FailMiserably));
            this.numOfCalls = 0;
            await Assert.ThrowsAsync<Exception>(
                async () => await RetryHelper.RunAndRetryWithValueAsync(
                                async () => { return await this.DoSomethingTestEnumValueAsync(TestEnum.Fail); },
                                nameof(this.RetryHelperValue_FailMiserably),
                                TestEnum.RetryValue,
                                loggingContext));
            Assert.Equal(1, this.numOfCalls);
        }

        private async Task<int> DoSomethingExceptionAsync(int succeedAt)
        {
            this.numOfCalls++;
            await Task.Delay(500); // This is just here to make it async.

            if (succeedAt == Fail)
            {
                throw new Exception();
            }

            if (this.numOfCalls == succeedAt)
            {
                return 0;
            }

            throw new ArgumentException();
        }

        private async Task<TestEnum> DoSomethingTestEnumValueAsync(TestEnum testEnum)
        {
            this.numOfCalls++;
            await Task.Delay(500); // This is just here to make it async.

            if (testEnum == TestEnum.Fail)
            {
                throw new Exception();
            }

            if (this.numOfCalls == 1 && testEnum == TestEnum.SucceedAtFirst)
            {
                return testEnum;
            }

            if (this.numOfCalls == 3 && testEnum == TestEnum.SucceedAtThird)
            {
                return testEnum;
            }

            return TestEnum.RetryValue;
        }
    }
}
