using Karambit.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Karambit.Web.HTTP
{
    public delegate void RequestEventHandler(object sender, RequestEventArgs e);

    public class HttpServer
    {
        #region Fields
        private TcpListener listener;
        private int port;
        private bool running;
        private Dictionary<ulong, HttpConnection> connections;
        private Serializer serializer;
        private string name;
        private Deployment deployment;

        private static Utilities.Random random;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the port.
        /// </summary>
        /// <value>The port.</value>
        public int Port {
            get {
                return port;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="HttpServer"/> is running.
        /// </summary>
        /// <value>
        ///   <c>true</c> if running; otherwise, <c>false</c>.
        /// </value>
        public bool Running {
            get {
                return running;
            }
        }

        /// <summary>
        /// Gets the serializer used for writing objects.
        /// </summary>
        /// <value>The serializer.</value>
        public Serializer Serializer {
            get {
                return serializer;
            }
            set {
                this.serializer = value;
            }
        }

        /// <summary>
        /// Gets or sets the server name.
        /// A value of null will prevent the header being sent.
        /// </summary>
        /// <value>The name.</value>
        public string Name {
            get {
                return name;
            }
            set {
                this.name = value;
            }
        }

        /// <summary>
        /// Gets or sets the deployment.
        /// </summary>
        /// <value>The deployment.</value>
        public Deployment Deployment {
            get {
                return deployment;
            }
            set {
                this.deployment = value;
            }
        }
        #endregion

        #region Events        
        /// <summary>
        /// Occurs when a the server receives a HTTP request.
        /// </summary>
        public event RequestEventHandler Request;

        /// <summary>
        /// Raises the <see cref="E:Request" /> event.
        /// </summary>
        /// <param name="e">The <see cref="RequestEventArgs"/> instance containing the event data.</param>
        internal virtual void OnRequest(RequestEventArgs e) {
            if (Request != null)
                Request(this, e);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Starts the HTTP server.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The HTTP server is already running</exception>
        public void Start() {
            // check if already running
            if (running)
                throw new InvalidOperationException("The HTTP server is already running");

            // start
            listener.Start();
            listener.BeginAcceptTcpClient(new AsyncCallback(Accept), null);
            
            // state
            running = true;
        }

        /// <summary>
        /// Accepts an incoming TCP client.
        /// </summary>
        /// <param name="res">The resource.</param>
        private void Accept(IAsyncResult res) {
            // client
            TcpClient client = listener.EndAcceptTcpClient(res);

            // create connection
            ulong id = random.RandomUInt64();
            HttpConnection conn = new HttpConnection(this, id, client);

            // add
            lock(connections)
                connections.Add(id, conn);

            // accept new
            listener.BeginAcceptTcpClient(new AsyncCallback(Accept), null);
        }

        /// <summary>
        /// Stops the HTTP server.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The HTTP server is not running</exception>
        public void Stop() {
            // check if actually running
            if (!running)
                throw new InvalidOperationException("The HTTP server is not running");

            // stop
            listener.Stop();

            // state
            running = false;
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Creates a new HTTP server on the provided port.
        /// </summary>
        /// <param name="port">The port.</param>
        public HttpServer(int port)
            : this(IPAddress.Any, port) {
        }

        /// <summary>
        /// Creates a new HTTP server on the provided port and address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="port">The port.</param>
        public HttpServer(IPAddress address, int port) {
            this.port = port;
            this.listener = new TcpListener(address, port);
            this.connections = new Dictionary<ulong, HttpConnection>();
            this.serializer = new EmptySerializer();
            this.name = "Karambit WS";
        }

        /// <summary>
        /// Initializes the <see cref="HttpServer"/> class.
        /// </summary>
        static HttpServer() {
            HttpServer.random = new Utilities.Random();
        }
        #endregion
    }
}