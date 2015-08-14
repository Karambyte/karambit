using System;
using System.Net;
using System.Net.Sockets;

namespace Karambit.Net
{
    public delegate void AcceptedEventHandler(object sender, AcceptedEventArgs e);

    public class Server : IServer
    {
        #region Fields
        protected bool running;
        protected int port;
        protected TcpListener listener;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets a value indicating whether this server is running.
        /// </summary>
        /// <value><c>true</c> if running; otherwise, <c>false</c>.</value>
        public bool Running {
            get {
                return running;
            }
        }

        /// <summary>
        /// Gets the port the server is running on.
        /// </summary>
        /// <value> The port.</value>
        public int Port {
            get {
                return port;
            }
        }
        #endregion

        #region Events
        public event AcceptedEventHandler Accepted;

        /// <summary>
        /// Raises the <see cref="E:Accepted" /> event.
        /// </summary>
        /// <param name="e">The <see cref="AcceptedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnAccepted(AcceptedEventArgs e) {
            if (Accepted != null)
                Accepted(this, e);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Starts the server.
        /// </summary>
        public virtual void Start() {
            if (running)
                throw new InvalidOperationException("The server is already running");

            // start listening
            listener.Start();

            // next
            listener.BeginAcceptTcpClient(new AsyncCallback(Accept), null);

            running = true;
        }

        protected virtual void Accept(IAsyncResult res) {
            // accept
            TcpClient client = listener.EndAcceptTcpClient(res);

            // trigger event
            OnAccepted(new AcceptedEventArgs(client));

            // next
            listener.BeginAcceptTcpClient(new AsyncCallback(Accept), null);
        }

        /// <summary>
        /// Stops the server.
        /// </summary>
        public virtual void Stop() {
            if (!running)
                throw new InvalidOperationException("The server is not running");

            // stop listening
            listener.Stop();

            running = false;
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="Server"/> class.
        /// </summary>
        /// <param name="port">The port.</param>
        public Server(int port) 
            : this(IPAddress.Any, port) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Server"/> class.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="port">The port.</param>
        public Server(IPAddress address, int port) {
            this.listener = new TcpListener(address, port);
        }
        #endregion
    }
}
