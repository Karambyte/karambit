using System;

namespace Karambit.Logging
{
    /// <summary>
    /// A class which represents a log message.
    /// </summary>
    public sealed class LogMessage
    {
        #region Fields
        private string message;
        private LogLevel level;
        private string channel;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message {
            get {
                return message;
            }
        }

        /// <summary>
        /// Gets the level.
        /// </summary>
        /// <value>The level.</value>
        public LogLevel Level {
            get {
                return level;
            }
        }

        /// <summary>
        /// Gets the channel.
        /// </summary>
        /// <value>The channel.</value>
        public string Channel {
            get {
                return channel;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessage"/> class.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="channel">The channel.</param>
        /// <param name="message">The message.</param>
        public LogMessage(LogLevel level, string channel, string message) {
            this.level = level;
            this.channel = channel;
            this.message = message;
        }
        #endregion
    }
}
