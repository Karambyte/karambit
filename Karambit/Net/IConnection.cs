using System;
using System.Net.Sockets;

namespace Karambit.Net
{
    /// <summary>
    /// An interface for objects which represent the serverside aspects of a client.
    /// </summary>
    public interface IConnection
    {
        /// <summary>
        /// Gets the underlying server.
        /// </summary>
        /// <value>The server.</value>
        IServer Server { get; }

        /// <summary>
        /// Gets the underlying client.
        /// </summary>
        /// <value>The client.</value>
        TcpClient Client { get; }
    }
}
