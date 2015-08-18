using Karambit.Net;
using Karambit.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Karambit.Web.HTTP
{
    public delegate void RequestEventHandler(object sender, RequestEventArgs e);
    public delegate void ErrorEventHandler(object sender, ErrorEventArgs e);

    public class HttpServer : Server
    {
        #region Fields
        private Dictionary<ulong, HttpConnection> connections;
        private Serializer serializer;
        private string name;

        private static Security.Random random;
        #endregion

        #region Properties
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
        #endregion

        #region Events
        /// <summary>
        /// Occurs when a the server receives a HTTP request.
        /// </summary>
        public event RequestEventHandler Request;

        /// <summary>
        /// Occurs when an error is about to be handled.
        /// </summary>
        public event ErrorEventHandler Error;

        /// <summary>
        /// Raises the <see cref="E:Request" /> event.
        /// </summary>
        /// <param name="e">The <see cref="RequestEventArgs"/> instance containing the event data.</param>
        internal virtual void OnRequest(RequestEventArgs e) {
            if (Request != null)
                Request(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:Error" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ErrorEventArgs"/> instance containing the event data.</param>
        internal virtual void OnError(ErrorEventArgs e) {
            if (Error != null)
                Error(this, e);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Accepts an incoming TCP client.
        /// </summary>
        /// <param name="res">The resource.</param>
        protected override void OnAccepted(AcceptedEventArgs e) {
            // create connection
            ulong id = random.RandomUInt64();
            HttpConnection conn = new HttpConnection(this, id, e.Client);

            // add
            lock(connections)
                connections.Add(id, conn);

            // base
            base.OnAccepted(e);
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
        public HttpServer(IPAddress address, int port) 
            : base(address, port) {
            this.connections = new Dictionary<ulong, HttpConnection>();
            this.serializer = new EmptySerializer();
            this.name = "Karambit WS";
        }

        /// <summary>
        /// Initializes the <see cref="HttpServer"/> class.
        /// </summary>
        static HttpServer() {
            HttpServer.random = new Security.Random();
        }
        #endregion
    }
}