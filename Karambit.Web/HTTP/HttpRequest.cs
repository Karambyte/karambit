using System;
using System.Collections.Generic;

namespace Karambit.Web.HTTP
{
    public class HttpRequest
    {
        #region Fields
        private HttpConnection connection;
        private string path;
        private HttpMethod method;
        private string version;
        private Dictionary<string, string> headers;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the underlying connection.
        /// </summary>
        /// <value>The connection.</value>
        public HttpConnection Connection {
            get {
                return connection;
            }
        }

        /// <summary>
        /// Gets the method.
        /// </summary>
        /// <value>The method.</value>
        public HttpMethod Method {
            get {
                return method;
            }
            internal set {
                this.method = value;
            }
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path {
            get {
                return path;
            }
            internal set {
                this.path = value;
            }
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
        public string Version {
            get {
                return version;
            }
            internal set {
                this.version = value;
            }
        }

        /// <summary>
        /// Gets the headers.
        /// </summary>
        /// <value>The headers.</value>
        public Dictionary<string, string> Headers {
            get {
                return headers;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequest"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public HttpRequest(HttpConnection connection) {
            this.connection = connection;
            this.headers = new Dictionary<string, string>();
            this.version = "";
            this.path = "";
        }
        #endregion
    }
}
