// -----------------------------------------------------------------------
// <copyright file="CertificateValidationHelperTest.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Winget.RestSource.UnitTest.Tests.Functions.Common
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Primitives;
    using Microsoft.WinGet.RestSource.Functions.Common;
    using Microsoft.Winget.RestSource.UnitTest.Common.Mocks;
    using Microsoft.Winget.RestSource.UnitTest.Tests.Utils.Common;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Exceptions;
    using Moq;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Certificate Validation Helper Test.
    /// </summary>
    public class CertificateValidationHelperTest : IDisposable
    {
        private const string Payload = "Payload";
        private const string CertHeader = "X-ARR-ClientCert";
        private const string CertSubject = "devauth";
        private const string Cert = "MIIDKjCCAhKgAwIBAgIQZCpkZ//ETPK6BVAsqAfmhjANBgkqhkiG9w0BAQsFADASMRAwDgYDVQQDEwdkZXZhdXRoMB4XDTIyMDcwNTIwNTQwNVoXDTMyMDcwNTIxMDQwNVowEjEQMA4GA1UEAxMHZGV2YXV0aDCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBANvLHTeBoQya1vMxmJl1xmd1+V1lGKFlUadAy3xGMIIvB5o7fbTMtv1PnZ9VF25DoYNGBNZznZYVck0ZXqX0FsEB5iA2gENg9sk5sOAF8pQnSYNSZx1ABCnLNpQfYYk0U/DziHbHCqc5sVIoI2TzoeRrPh4W5kXLloys4anY15LrR++lEmiDDnpL8WUR2XKia28/UWEG84xMbz4WZ0J0yltmfw2xigWFf+BPqDJVBJ4MTW62BXIKjaJT7XX5EMWGnPZz7BlznrQqcRw845gGFNjDN8bvdKmx9IVOhQeOBfvZmo9jARrk+ODO97o32jQOfYDKyWdNEXOPZrD6AZx4hG0CAwEAAaN8MHowDgYDVR0PAQH/BAQDAgWgMAkGA1UdEwQCMAAwHQYDVR0lBBYwFAYIKwYBBQUHAwEGCCsGAQUFBwMCMB8GA1UdIwQYMBaAFEMiX/5DITRc8rm2HQz63EAqtXPZMB0GA1UdDgQWBBRDIl/+QyE0XPK5th0M+txAKrVz2TANBgkqhkiG9w0BAQsFAAOCAQEA1Xc0Mo1Ex6aXlZHkfJn0yZVa/+6z0QFK95D+9wWZ4K5v4lGvANLNxU28IlJyg3kJ8kV720WR4NYsyCcn6mWxNHYKxivO/xhoDsIgIllsJcvC4GWeUx+67xeNPQuSH20IZuNSqMoqpD5+aedzk24pVkWudIz5sEWE0+TGik4y6DeqWIhNIzSphlUYODaAbMTFn40rI4Afugv6518FkVFyZKYX/mDgD/jn2RarOph2k3ffJY3gclQ0yH4DggAA82vF35j2B5xKzBBnuF5AlojjDM9oHLukVYDtGs5yAfMAebRK7TAtcQjpzIPZv7jseW7LBBdKO5A9cQxKEBAIOa33Dw==";
        private const string UserAgentHeaderTitle = "User-Agent";
        private const string UserAgentHeaderValue = "Agent2";
        private readonly ITestOutputHelper log;
        private readonly Mock<ILogger> loggerMock = new Mock<ILogger>();
        private EnvironmentVariables env;

        /// <summary>
        /// Initializes a new instance of the <see cref="CertificateValidationHelperTest"/> class.
        /// </summary>
        /// <param name="log">ITestOutputHelper.</param>
        public CertificateValidationHelperTest(ITestOutputHelper log)
        {
            this.log = log;
            Dictionary<string, string> environmentVars = new Dictionary<string, string>()
            {
                [ApiConstants.CertificateAuthenticationRequiredEnvName] = "false",
                [ApiConstants.CertificateAuthenticationSubjectNameEnvName] = CertSubject,
            };
            this.env = EnvironmentVariables.PrepareEnvironmentVariable(environmentVars);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            EnvironmentVariables.RestoreEnvironmentVariables(this.env, new List<string>()
            {
                ApiConstants.CertificateAuthenticationRequiredEnvName,
            });
        }

        /// <summary>
        /// This verifies that the auth function exits successfully when disabled.
        /// </summary>
        [Fact]
        public void DisabledAuth()
        {
            // Disable Cert Auth
            this.env.BackupIfNeededAndReplace(ApiConstants.CertificateAuthenticationRequiredEnvName, "false");

            // Execute Validation
            Mock<HttpRequest> request = MoqHTTPRequest.CreateMockRequest(
                Payload,
                new HeaderDictionary(new Dictionary<string, StringValues>
                {
                    { UserAgentHeaderTitle, UserAgentHeaderValue },
                }));
            CertificateValidationHelper.ValidateAuthentication(null, null);
        }

        /// <summary>
        /// This verifies that the auth function exits successfully when disabled.
        /// </summary>
        [Fact]
        public void EnabledAuthNoCert()
        {
            // Enable Cert Auth
            this.env.BackupIfNeededAndReplace(ApiConstants.CertificateAuthenticationRequiredEnvName, "true");

            // Create Payload
            Mock<HttpRequest> request = MoqHTTPRequest.CreateMockRequest(
                Payload,
                new HeaderDictionary(new Dictionary<string, StringValues>
                {
                    { UserAgentHeaderTitle, UserAgentHeaderValue },
                }));

            // Verify Failure
            Assert.Throws<ForbiddenException>(() => CertificateValidationHelper.ValidateAuthentication(request.Object, null));
        }

        /// <summary>
        /// This verifies that the auth function exits successfully when disabled.
        /// </summary>
        [Fact]
        public void EnabledAuthCertValidCertNotConfigured()
        {
            // Disable Cert Auth
            this.env.BackupIfNeededAndReplace(ApiConstants.CertificateAuthenticationRequiredEnvName, "true");

            // Create Payload
            Mock<HttpRequest> request = MoqHTTPRequest.CreateMockRequest(
                Payload,
                new HeaderDictionary(new Dictionary<string, StringValues>
                {
                    { UserAgentHeaderTitle, UserAgentHeaderValue },
                    { CertHeader, Cert },
                }));

            // Verify Failure
            Assert.Throws<ForbiddenException>(() => CertificateValidationHelper.ValidateAuthentication(request.Object, this.loggerMock.Object));
        }
    }
}
