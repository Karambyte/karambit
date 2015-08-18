using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Karambit.Web.HTTP
{
    public class HttpResponse
    {
        #region Fields
        protected HttpConnection connection;
        protected HttpClient client;
        protected MemoryStream buffer;
        protected HttpStatus status;
        protected string version = "HTTP/1.1";
        protected Dictionary<string, string> headers;
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
        /// Gets the server which the response is being sent from.
        /// This value will be null if the response was created by a HttpClient object.
        /// </summary>
        /// <value>The server.</value>
        public HttpServer Server {
            get {
                return (connection == null) ? null : (HttpServer)connection.Server;
            }
        }

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>The status code.</value>
        public HttpStatus StatusCode {
            get {
                return status;
            }
            set {
                this.status = value;
            }
        }

        /// <summary>
        /// Gets the underlying buffer.
        /// </summary>
        /// <value>The buffer.</value>
        public MemoryStream Buffer {
            get {
                return buffer;
            }
            internal set {
                this.buffer = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>The type of the content.</value>
        public string ContentType {
            get {
                return headers["Content-Type"];
            } set {
                this.headers["Content-Type"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        public string Location {
            get {
                return headers.ContainsKey("Location") ? headers["Location"] : "";
            }
            set {
                this.headers["Location"] = value;
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
        /// Gets or sets the version.
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
        #endregion

        #region Methods
        /// <summary>
        /// Clears the response buffer.
        /// </summary>
        public void Clear() {
            buffer = new MemoryStream();
        }

        /// <summary>
        /// Writes the specified string to the buffer.
        /// </summary>
        /// <param name="str">The string.</param>
        public void Write(string str) {
            // get bytes
            byte[] bytes = Encoding.UTF8.GetBytes(str);

            // write
            buffer.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes the specified data to the buffer.
        /// </summary>
        /// <param name="data">The data.</param>
        public void Write(byte[] data) {
            // write
            buffer.Write(data, 0, data.Length);
        }

        /// <summary>
        /// Writes the specified bytes to the buffer.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The number of bytes.</param>
        public void Write(byte[] data, int offset, int count) {
            buffer.Write(data, offset, count);
        }

        /// <summary>
        /// Writes the specified object with the server's selected serializer.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void Write(object obj) {
            Write(((HttpServer)connection.Server).Serializer.Serialize(obj));
        }

        /// <summary>
        /// Writes a file to the buffer.
        /// </summary>
        /// <param name="path">The path.</param>
        public void WriteFile(string path) {
            // read file
            byte[] data = File.ReadAllBytes(path);

            // write
            buffer.Write(data, 0, data.Length);
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpResponse"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        internal HttpResponse(IHttpTransaction source) {
            if (source is HttpClient)
                this.client = (HttpClient)source;
            else
                this.connection = (HttpConnection)source;

            this.status = HttpStatus.OK;
            this.buffer = new MemoryStream();
            this.headers = new Dictionary<string, string>() {
                
            };

            if (source is HttpConnection) {
                headers.Add("Content-Type", "text/html");
                headers.Add("Connection", "Keep-Alive");
                headers.Add("X-Frame-Options", "deny");
            }
        }
        #endregion
    }
}
