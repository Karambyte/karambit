using System;
using System.Collections.Generic;

namespace Karambit.Web.HTTP
{
    public class HttpRequest
    {
        #region Fields
        protected HttpConnection connection;
        protected HttpClient client;
        protected string path;
        protected HttpMethod method;
        protected string version;
        protected Dictionary<string, string> headers;
        protected Dictionary<string, string> parameters;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the underlying connection.
        /// This value will be null if the response is being sent to a HttpClient object.
        /// </summary>
        /// <value>The connection.</value>
        public HttpConnection Connection {
            get {
                return connection;
            }
        }

        /// <summary>
        /// Gets the client.
        /// This value will be null if the response is being sent by a HttpServer object.
        /// </summary>
        /// <value>The client.</value>
        public HttpClient Client {
            get {
                return client;
            }
        }

        /// <summary>
        /// Gets the server which the request was sent to.
        /// This value will be null if the requested was created by a HttpClient object.
        /// </summary>
        /// <value>The server.</value>
        public HttpServer Server {
            get {
                return (connection == null) ? null : connection.Server;
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
            set {
                this.method = value;
            }
        }

        /// <summary>
        /// Gets the user agent.
        /// </summary>
        /// <value>The user agent.</value>
        public string UserAgent {
            get {
                return headers.ContainsKey("user-agent") ? headers["user-agent"] : "";
            }
        }

        /// <summary>
        /// Gets the hostname.
        /// </summary>
        /// <value>The hostname.</value>
        public string Host {
            get {
                return headers.ContainsKey("host") ? headers["host"] : "";
            }
            set {
                headers["Host"] = value;
            }
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path {
            get {
                return path;
            } set {
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
            } internal set {
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
            internal set {
                // check headers
                if (headers.Count > 0)
                    throw new InvalidOperationException("The attempted operation would omit required headers");

                this.headers = value;
            }
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public Dictionary<string, string> Parameters {
            get {
                return parameters;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequest"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        internal HttpRequest(IHttpTransaction source) {
            if (source is HttpClient)
                this.client = (HttpClient)source;
            else
                this.connection = (HttpConnection)source;

            this.headers = new Dictionary<string, string>();
            this.parameters = new Dictionary<string, string>();
            this.version = "";
            this.path = "";
        }
        #endregion
    }
}
