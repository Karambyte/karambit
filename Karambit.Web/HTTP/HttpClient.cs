using System;
using System.Net.Sockets;

namespace Karambit.Web.HTTP
{
    public class HttpClient : IHttpTransaction
    {
        #region Fields
        private string host;
        private int port;
        protected TcpClient client;
        protected HttpWriter writer;
        protected HttpReader reader;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the host.
        /// </summary>
        /// <value>The host.</value>
        public string Host {
            get {
                return host;
            }
        }

        /// <summary>
        /// Gets the port.
        /// </summary>
        /// <value>The port.</value>
        public int Port {
            get {
                return port;
            }
        }
        #endregion

        #region Methods        
        /// <summary>
        /// Connects/reconnects to the server.
        /// </summary>
        private void Connect() {
            client = new TcpClient(host, port);

            NetworkStream stream = client.GetStream();
            reader = new HttpReader(stream, this);
            writer = new HttpWriter(stream, this);

            Application.Logger.Log(Logging.LogLevel.Information, "debug", "connected to " + host + ":" + port);
        }

        /// <summary>
        /// Creates a new request for the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public HttpRequest Create(HttpMethod method, string path) {
            HttpRequest req = new HttpRequest(this);
            req.Method = method;
            req.Path = path;
            req.Host = host;
            return req;
        }

        public HttpRequest Get(string path) {
            return Create(HttpMethod.GET, path);
        }

        public HttpRequest Post(string path) {
            return Create(HttpMethod.POST, path);
        }

        public HttpRequest Put(string path) {
            return Create(HttpMethod.PUT, path);
        }

        public HttpRequest Delete(string path) {
            return Create(HttpMethod.DELETE, path);
        }

        public HttpResponse Send(HttpRequest req) {
            // reconnect if necessary
            if (!client.Connected)
                Connect();

            // write request
            writer.WriteRequest(req);

            // read response
            return reader.ReadResponse();
        }
        #endregion

        #region Constructors
        public HttpClient(string host, int port) {
            this.host = host;
            this.port = port;
            this.Connect();
        }
        #endregion
    }
}
