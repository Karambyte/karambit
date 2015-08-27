﻿using System;

namespace Karambit.Net
{
    /// <summary>
    /// An interface that represents a server which can accept connections and runs on a networking port.
    /// </summary>
    public interface IServer
    {
        /// <summary>
        /// Starts the server.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the server.
        /// </summary>
        void Stop();

        /// <summary>
        /// Gets a value indicating whether this <see cref="IServer"/> is running.
        /// </summary>
        /// <value><c>true</c> if running; otherwise, <c>false</c>.</value>
        bool Running { get; }

        /// <summary>
        /// Gets the port the server is running on.
        /// </summary>
        /// <value>The port.</value>
        int Port { get; }
    }
}
