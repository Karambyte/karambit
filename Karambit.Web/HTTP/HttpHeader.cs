using System;
using System.Collections.Generic;

namespace Karambit.Web.HTTP
{
    public class HttpHeader
    {
        #region Fields
        private string name;
        private object value;

        private static List<string> _standardHeaders = new List<string> { 
            "accept", "accept-charset", "accept-encoding", "accept-language",
            "accept-datetime", "authorization", "cache-control", "connection",
            "cookie", "content-length", "content-md5", "content-type", "date",
            "expect", "from", "host", "if-match", "if-modified-since", "if-none-match",
            "if-range", "if-unmodified-since", "max-forwards", "origin", "pragma",
            "proxy-authorization", "range", "referer", "referrer", "te", "user-agent",
            "upgrade", "via", "warning", "dnt", "access-control-allow-origin", "accept-patch",
            "accept-ranges", "age", "allow","content-disposition",
            "content-encoding", "content-language", "content-location","content-range", "etag",
            "expires", "last-modified", "link", "location", "p3p", "proxy-authenticate", 
            "public-key-pins", "refresh", "retry-after", "server", "set-cookie", "status",
            "strict-transport-security", "trailer", "transfer-encoding", "www-authenticate",
            "x-frame-options"
        };
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the header name.
        /// </summary>
        /// <value>The name.</value>
        public string Name {
            get {
                return name;
            }
        }

        /// <summary>
        /// Gets the header value.
        /// </summary>
        /// <value>The value.</value>
        public object Value {
            get {
                return value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Determines whether this is a standardised header.
        /// </summary>
        /// <returns></returns>
        public bool IsStandard() {
            return _standardHeaders.Contains(name.ToLower());
        }

        /// <summary>
        /// Determines whether this is a karambit-specific header.
        /// </summary>
        /// <returns></returns>
        public bool IsKarambit() {
            return name.ToLower().StartsWith("x-karambit");
        }

        /// <summary>
        /// Parses the header string dependant on the header name.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private object Parse(string value) {
            switch (name.ToLower()) {
                case "content-length":
                    return long.Parse(value);
                case "max-forwards":
                    return int.Parse(value);
                default:
                    return value;
            }
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHeader"/> class with an unparsed value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public HttpHeader(string name, string value) {
            this.name = name;
            this.value = Parse(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHeader"/> class with a value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public HttpHeader(string name, object value) {
            this.name = name;
            this.value = (value is String) ? Parse((string)value) : value;
        }
        #endregion
    }
}
