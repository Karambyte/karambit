using System;
using System.Net.Sockets;

namespace Karambit.Net
{
    public class AcceptedEventArgs : EventArgs
    {
        #region Fields
        private TcpClient client;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>The client.</value>
        public TcpClient Client {
            get {
                return client;
            } set {
                this.client = value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AcceptedEventArgs"/> class with the relevant TCP client.
        /// </summary>
        /// <param name="client">The client.</param>
        public AcceptedEventArgs(TcpClient client) {
            this.client = client;
        }
        #endregion
    }
}
