// -----------------------------------------------------------------------
// <copyright file="StringEncoder.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Common
{
    /// <summary>
    /// String Encoder.
    /// </summary>
    public static class StringEncoder
    {
        /// <summary>
        /// This will encode a string to Base 64.
        /// </summary>
        /// <param name="plainText">Plain Text.</param>
        /// <returns>Encoded data.</returns>
        public static string Base64Encode(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                return null;
            }

            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// This will decode data from Base 64.
        /// </summary>
        /// <param name="base64EncodedData">Encoded data.</param>
        /// <returns>Plain Text.</returns>
        public static string Base64Decode(string base64EncodedData)
        {
            if (string.IsNullOrEmpty(base64EncodedData))
            {
                return null;
            }

            byte[] base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// Encodes a Continuation Token.
        /// </summary>
        /// <param name="token">Token.</param>
        /// <returns>Encoded Token.</returns>
        public static string EncodeContinuationToken(string token)
        {
            return Base64Encode(token);
        }

        /// <summary>
        /// Decode a Continuation Token.
        /// </summary>
        /// <param name="encodedToken">Encoded Token.</param>
        /// <returns>Token.</returns>
        public static string DecodeContinuationToken(string encodedToken)
        {
            return string.IsNullOrEmpty(encodedToken) ? null : Base64Decode(encodedToken);
        }
    }
}