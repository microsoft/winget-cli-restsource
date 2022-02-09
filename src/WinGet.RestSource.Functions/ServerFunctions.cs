// -----------------------------------------------------------------------
// <copyright file="ServerFunctions.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Functions
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Primitives;
    using Microsoft.WinGet.RestSource.Cosmos;
    using Microsoft.WinGet.RestSource.Functions.Common;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Exceptions;
    using Microsoft.WinGet.RestSource.Utils.Models;
    using Microsoft.WinGet.RestSource.Utils.Models.Errors;
    using Microsoft.WinGet.RestSource.Utils.Models.Schemas;

    /// <summary>
    /// This class contains the functions for interacting with packages.
    /// </summary>
    public class ServerFunctions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerFunctions"/> class.
        /// </summary>
        public ServerFunctions()
        {
        }

        /// <summary>
        /// Server Information Get Function.
        /// This allows us to make Get Server Information.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.InformationGet)]
        public IActionResult InformationGetAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, FunctionConstants.FunctionGet, Route = "information")]
            HttpRequest req,
            ILogger log)
        {
            // TODO: Move into helper. Splash across all.
            if (ApiConstants.CertificateAuthenticationRequired)
            {
                StringValues certHeader;
                bool headerPresent = req.Headers.TryGetValue("X-ARR-ClientCert", out certHeader);

                if (!headerPresent || StringValues.IsNullOrEmpty(certHeader))
                {
                    // TODO fix
                    log.LogInformation("MISSING CERT HEADER");
                    var test = new ApiObjectResult(
                        new InternalRestError(ErrorConstants.UnhandledErrorCode, "Cert header was missing"),
                        (int)HttpStatusCode.Forbidden);
                    return test;
                }

                byte[] clientCertBytes = Convert.FromBase64String(certHeader);
                X509Certificate2 certificate = new X509Certificate2(clientCertBytes);

                // TODO: Pass logging?
                // TODO: Refactor helper.
                log.LogInformation("TESTING CERT");
                if (!this.IsClientCertValid(certificate, log))
                {
                    // TODO fix
                    log.LogInformation("TESTING CERT FAILED");
                    var test = new ApiObjectResult(
                        new InternalRestError(ErrorConstants.UnhandledErrorCode, "Cert header failed validation"),
                        (int)HttpStatusCode.Forbidden);
                    return test;
                }
            }

            Information information;
            try
            {
                information = new Information();
            }
            catch (DefaultException e)
            {
                log.LogError(e.ToString());
                return ActionResultHelper.ProcessError(e.InternalRestError);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return ActionResultHelper.UnhandledError(e);
            }

            return new ApiObjectResult(new ApiResponse<Information>(information));
        }

        private bool IsClientCertValid(X509Certificate2 certificate, ILogger log)
        {
            // Do we have a certificate?
            if (certificate == null)
            {
                return false;
            }

            // Is the Certificate not expired?
            if (DateTime.Compare(DateTime.Now, certificate.NotBefore) < 0 || DateTime.Compare(DateTime.Now, certificate.NotAfter) > 0)
            {
                return false;
            }

            // Fetch server root certificate to perform comparison and chaining
            // TODO env variable.
            string certSubject = "wingetsourcetestroot";
            bool validOnly = false;
            using (X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            {
                certStore.Open(OpenFlags.ReadOnly);

                X509Certificate2Collection certCollection = certStore.Certificates.Find(
                    X509FindType.FindBySubjectName,
                    certSubject,
                    validOnly);

                X509Certificate2 serverCert = certCollection.OfType<X509Certificate2>().FirstOrDefault();

                if (serverCert is null)
                {
                    log.LogInformation("CERT FAILED TO FIND SERVER CERT");
                    return false;
                }

                log.LogInformation($"CERT server cert info: {serverCert.Thumbprint} {serverCert.Issuer} {serverCert.SubjectName}");

                // Does the certificate subject name match our root subject name?
                // TODO should this match or look at alt subject? or?
                log.LogInformation("CERT CHECK SUBJECT");
                bool foundSubject = false;
                string[] certSubjectData = certificate.Subject.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string subject in certSubjectData)
                {
                    log.LogInformation($"CERT Subject {subject}");
                    if (string.Compare(subject.Trim(), "CN=wingetsourcetestcertSignedByRootCA") == 0)
                    {
                        foundSubject = true;
                        break;
                    }
                }

                if (!foundSubject)
                {
                    return false;
                }

                // Does the certificate issuer name match our root issuer name?
                bool foundIssuerCN = false;
                log.LogInformation("CERT CHECK ISSUER");
                string[] certIssuerData = certificate.Issuer.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string issuer in certIssuerData)
                {
                    log.LogInformation($"CERT Issuer {issuer}");
                    if (string.Compare(issuer.Trim(), "CN=wingetsourcetestroot") == 0)
                    {
                        foundIssuerCN = true;
                    }
                }

                if (!foundIssuerCN)
                {
                    log.LogInformation($"CERT Issuer fail");
                    return false;
                }

                // Does the certificate chain of trust to our root certificate?
                var chain = new X509Chain();
                chain.ChainPolicy.ExtraStore.Add(serverCert);

                // Disable revocation check as this is selfsigned.
                // TODO: How will we demonstrate/fork this code for non-selfsigned cert example?
                chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;

                // Explicitly allow unknown roots since we are testing a self signed cert.
                chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;

                if (!chain.Build(certificate))
                {
                    log.LogInformation("CERT Failed to build chain to server root cert. TODO log more details about chain build.");
                    log.LogInformation("Chain Information");
                    log.LogInformation("Chain revocation flag: {0}", chain.ChainPolicy.RevocationFlag);
                    log.LogInformation("Chain revocation mode: {0}", chain.ChainPolicy.RevocationMode);
                    log.LogInformation("Chain verification flag: {0}", chain.ChainPolicy.VerificationFlags);
                    log.LogInformation("Chain verification time: {0}", chain.ChainPolicy.VerificationTime);
                    log.LogInformation("Chain status length: {0}", chain.ChainStatus.Length);
                    log.LogInformation("Chain application policy count: {0}", chain.ChainPolicy.ApplicationPolicy.Count);
                    log.LogInformation("Chain certificate policy count: {0}", chain.ChainPolicy.CertificatePolicy.Count);

                    log.LogInformation("Chain Element Information");
                    log.LogInformation("Number of chain elements: {0}", chain.ChainElements.Count);
                    log.LogInformation("Chain elements synchronized? {0}", chain.ChainElements.IsSynchronized);

                    foreach (X509ChainElement element in chain.ChainElements)
                    {
                        log.LogInformation("Element issuer name: {0}", element.Certificate.Issuer);
                        log.LogInformation("Element certificate valid until: {0}", element.Certificate.NotAfter);
                        log.LogInformation("Element certificate is valid: {0}", element.Certificate.Verify());
                        log.LogInformation("Element error status length: {0}", element.ChainElementStatus.Length);
                        log.LogInformation("Element information: {0}", element.Information);
                        log.LogInformation("Number of element extensions: {0}", element.Certificate.Extensions.Count);

                        if (chain.ChainStatus.Length > 1)
                        {
                            for (int index = 0; index < element.ChainElementStatus.Length; index++)
                            {
                                log.LogInformation(element.ChainElementStatus[index].Status.ToString());
                                log.LogInformation(element.ChainElementStatus[index].StatusInformation);
                            }
                        }
                    }

                    return false;
                }

                // Selfsigned: If we have a valid chain, double check that it chained to the item we planned to chain to.
                // For non selfsigned, we shouldn't need to do this. Though, we might want to verify that our intermediate cert is on the chain.
                // TODO
                var chainRoot = chain.ChainElements[chain.ChainElements.Count - 1].Certificate;
                if (!chainRoot.RawData.SequenceEqual(serverCert.RawData))
                {
                    log.LogInformation("CERT servercert wasn't the root of the chain.");
                    return false;
                }
            }

            return true;
        }
    }
}
