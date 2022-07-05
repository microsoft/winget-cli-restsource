// -----------------------------------------------------------------------
// <copyright file="CertificateValidationHelper.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Functions.Common
{
    using System;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Primitives;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Exceptions;
    using Microsoft.WinGet.RestSource.Utils.Models.Errors;

    /// <summary>
    /// Provides common certificate validation helpers.
    /// </summary>
    public static class CertificateValidationHelper
    {
        /// <summary>
        /// This will check for a client certificate and verify its applicability.
        /// </summary>
        /// <param name="req">the httprequest to process.</param>
        /// <param name="log">Log output.</param>
        public static void ValidateAuthentication(HttpRequest req, ILogger log)
        {
            if (ApiConstants.CertificateAuthenticationRequired)
            {
                StringValues certHeader;
                bool headerPresent = req.Headers.TryGetValue("X-ARR-ClientCert", out certHeader);

                if (!headerPresent || StringValues.IsNullOrEmpty(certHeader))
                {
                    throw new ForbiddenException(
                        new InternalRestError(
                            ErrorConstants.ForbiddenErrorCode,
                            string.Format(ErrorConstants.ForbiddenErrorMessage, ApiConstants.CertificateAuthenticationSubjectName)));
                }

                byte[] clientCertBytes = Convert.FromBase64String(certHeader);
                X509Certificate2 certificate = new X509Certificate2(clientCertBytes);

                if (!IsClientCertValid(certificate, log))
                {
                    throw new ForbiddenException(
                        new InternalRestError(
                            ErrorConstants.ForbiddenErrorCode,
                            string.Format(ErrorConstants.ForbiddenErrorMessage, ApiConstants.CertificateAuthenticationSubjectName)));
                }
            }
        }

        /// <summary>
        /// Checks the validity of a given client certificate.
        /// </summary>
        /// <param name="certificate">Certificate to validate.</param>
        /// <param name="log">Log output.</param>
        /// <returns>True if the certificate is valid. False otherwise.</returns>
        private static bool IsClientCertValid(X509Certificate2 certificate, ILogger log)
        {
            // Do we have a certificate?
            if (certificate == null)
            {
                log.LogInformation("No client certificate found.");
                return false;
            }

            // Is the Certificate not expired?
            if (DateTime.Compare(DateTime.Now, certificate.NotBefore) < 0 || DateTime.Compare(DateTime.Now, certificate.NotAfter) > 0)
            {
                log.LogInformation("Client certificate is expired.");
                return false;
            }

            // Fetch server root certificate to perform comparison and chaining
            bool validOnly = false;
            using (X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            {
                certStore.Open(OpenFlags.ReadOnly);

                X509Certificate2Collection certCollection = certStore.Certificates.Find(
                    X509FindType.FindBySubjectName,
                    ApiConstants.CertificateAuthenticationSubjectName,
                    validOnly);

                X509Certificate2 serverCert = certCollection.OfType<X509Certificate2>().FirstOrDefault();

                if (serverCert is null)
                {
                    log.LogError("No configured server certificate found.");
                    return false;
                }

                // Note: We skip subject name validation for client cert as the subject may differ per client machine.
                // Our primary validation is that the cert itself was signed by the server certificate.
                if (string.Compare(certificate.Issuer, serverCert.Subject) != 0)
                {
                    log.LogInformation("Client certificate is not issued by the expected issuer.");
                    return false;
                }

                // Does the certificate chain of trust to our root certificate?
                var chain = new X509Chain();
                chain.ChainPolicy.ExtraStore.Add(serverCert);

                if (ApiConstants.CertificateAuthenticationSelfSigned)
                {
                    // Disable revocation check as this is selfsigned.
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;

                    // Explicitly allow unknown roots since we are testing a self signed cert.
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;
                }

                if (!chain.Build(certificate))
                {
                    // If the chain failed, dump diagnostic information to trace logging.
                    log.LogTrace("Chain revocation flag: {0}", chain.ChainPolicy.RevocationFlag);
                    log.LogTrace("Chain revocation mode: {0}", chain.ChainPolicy.RevocationMode);
                    log.LogTrace("Chain verification flag: {0}", chain.ChainPolicy.VerificationFlags);
                    log.LogTrace("Chain verification time: {0}", chain.ChainPolicy.VerificationTime);
                    log.LogTrace("Chain status length: {0}", chain.ChainStatus.Length);
                    log.LogTrace("Chain application policy count: {0}", chain.ChainPolicy.ApplicationPolicy.Count);
                    log.LogTrace("Chain certificate policy count: {0}", chain.ChainPolicy.CertificatePolicy.Count);

                    log.LogTrace("Number of chain elements: {0}", chain.ChainElements.Count);
                    log.LogTrace("Chain elements synchronized? {0}", chain.ChainElements.IsSynchronized);

                    foreach (X509ChainElement element in chain.ChainElements)
                    {
                        log.LogTrace("Element issuer name: {0}", element.Certificate.Issuer);
                        log.LogTrace("Element certificate valid until: {0}", element.Certificate.NotAfter);
                        log.LogTrace("Element certificate is valid: {0}", element.Certificate.Verify());
                        log.LogTrace("Element error status length: {0}", element.ChainElementStatus.Length);
                        log.LogTrace("Element information: {0}", element.Information);
                        log.LogTrace("Number of element extensions: {0}", element.Certificate.Extensions.Count);

                        if (chain.ChainStatus.Length > 1)
                        {
                            for (int index = 0; index < element.ChainElementStatus.Length; index++)
                            {
                                log.LogTrace(element.ChainElementStatus[index].Status.ToString());
                                log.LogTrace(element.ChainElementStatus[index].StatusInformation);
                            }
                        }
                    }

                    return false;
                }

                if (ApiConstants.CertificateAuthenticationSelfSigned)
                {
                    // Selfsigned: If we have a valid chain, double check that it chained to the item we planned to chain to.
                    var chainRoot = chain.ChainElements[chain.ChainElements.Count - 1].Certificate;
                    if (!chainRoot.RawData.SequenceEqual(serverCert.RawData))
                    {
                        log.LogInformation("Server certificate was not the root was the chain.");
                        return false;
                    }
                }
                else
                {
                    // Check that the server certificate is present in the certificate chain.
                    bool found = false;
                    foreach (var element in chain.ChainElements)
                    {
                        if (element.Certificate.RawData.SequenceEqual(serverCert.RawData))
                        {
                            found = true;
                        }
                    }

                    if (!found)
                    {
                        log.LogInformation("Server certificate was not found in the client certificate chain of trust.");
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
