// -----------------------------------------------------------------------
// <copyright file="UnsupportedAndRequiredFieldsHelper.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Common
{
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using Microsoft.WinGet.RestSource.Constants;
    using Microsoft.WinGet.RestSource.Models.Arrays;
    using Microsoft.WinGet.RestSource.Models.Schemas;

    /// <summary>
    /// Unsupported and required fields helper.
    /// </summary>
    public static class UnsupportedAndRequiredFieldsHelper
    {
        /// <summary>
        /// Helper method to get the list of unsupported package match fields from search request.
        /// </summary>
        /// <param name="manifestSearchRequest">Manifest search request.</param>
        /// <param name="unsupportedPackageMatchFields">List of unsupported package match fields by server.</param>
        /// <returns>List of unsupported package match fields.</returns>
        public static PackageMatchFields GetUnsupportedPackageMatchFieldsFromSearchRequest(
            ManifestSearchRequest manifestSearchRequest, PackageMatchFields unsupportedPackageMatchFields)
        {
            PackageMatchFields unsupportedList = new PackageMatchFields();
            foreach (var field in unsupportedPackageMatchFields)
            {
                if ((manifestSearchRequest.Inclusions != null && manifestSearchRequest.Inclusions.Any(x => x.PackageMatchField.ToLower().Equals(field.ToLower())))
                    || (manifestSearchRequest.Filters != null && manifestSearchRequest.Filters.Any(x => x.PackageMatchField.ToLower().Equals(field.ToLower()))))
                {
                    unsupportedList.Add(field);
                }
            }

            return unsupportedList;
        }

        /// <summary>
        /// Helper method to get the list of required package match fields from search request.
        /// </summary>
        /// <param name="manifestSearchRequest">Manifest search request.</param>
        /// <param name="requiredPackageMatchFields">List of required package match fields by server.</param>
        /// <returns>List of required package match fields.</returns>
        public static PackageMatchFields GetRequiredPackageMatchFieldsFromSearchRequest(
            ManifestSearchRequest manifestSearchRequest, PackageMatchFields requiredPackageMatchFields)
        {
            PackageMatchFields requiredFields = new PackageMatchFields();
            if ((manifestSearchRequest.Inclusions == null || manifestSearchRequest.Inclusions.Count == 0) &&
                (manifestSearchRequest.Filters == null || manifestSearchRequest.Filters.Count == 0))
            {
                return requiredPackageMatchFields;
            }

            foreach (var field in requiredPackageMatchFields)
            {
                if (!((manifestSearchRequest.Inclusions != null && manifestSearchRequest.Inclusions.Any(x => x.PackageMatchField.ToLower().Equals(field.ToLower())))
                    || (manifestSearchRequest.Filters != null && manifestSearchRequest.Filters.Any(x => x.PackageMatchField.ToLower().Equals(field.ToLower())))))
                {
                    requiredFields.Add(field);
                }
            }

            return requiredFields;
        }

        /// <summary>
        /// Helper method to get the list of unsupported query parameters from the request.
        /// </summary>
        /// <param name="queryParameters">Query parameters input.</param>
        /// <param name="unsupportedQueryParameters">List of unsupported query parameters by server.</param>
        /// <returns>List of unsupported query parameters.</returns>
        public static QueryParameters GetUnsupportedQueryParametersFromRequest(
            IQueryCollection queryParameters, QueryParameters unsupportedQueryParameters)
        {
            QueryParameters unsupportedList = new QueryParameters();

            if (queryParameters == null || queryParameters.Count == 0)
            {
                return unsupportedList;
            }

            foreach (var field in unsupportedQueryParameters)
            {
                if (queryParameters.ContainsKey(field))
                {
                    unsupportedList.Add(field);
                }
            }

            return unsupportedList;
        }

        /// <summary>
        /// Helper method to get the list of required query parameters from the request.
        /// </summary>
        /// <param name="queryParameters">Query parameters input.</param>
        /// <param name="requiredQueryParameters">List of required query parameters by server.</param>
        /// <returns>List of required query parameters.</returns>
        public static QueryParameters GetRequiredQueryParametersFromRequest(
            IQueryCollection queryParameters, QueryParameters requiredQueryParameters)
        {
            QueryParameters requiredList = new QueryParameters();
            if (queryParameters == null || queryParameters.Count == 0)
            {
                return requiredQueryParameters;
            }

            foreach (var field in requiredQueryParameters)
            {
                if (!queryParameters.ContainsKey(field))
                {
                    requiredList.Add(field);
                }
            }

            return requiredList;
        }
    }
}
