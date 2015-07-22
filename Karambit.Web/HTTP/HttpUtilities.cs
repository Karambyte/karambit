using System;
using System.Net;

namespace Karambit.Web.HTTP
{
    public class HttpUtilities
    {
        #region Methods        
        /// <summary>
        /// Encodes the URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static string EncodeURL(string url) {
            return WebUtility.UrlEncode(url);
        }

        /// <summary>
        /// Decodes the URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static string DecodeURL(string url) {
            return WebUtility.UrlDecode(url);
        }
        #endregion
    }
}
