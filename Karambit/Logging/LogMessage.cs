using System;

namespace Karambit.Logging
{
    public class LogMessage
    {
        #region Fields
        public string message;
        public LogLevel level;
        public string channel;
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
