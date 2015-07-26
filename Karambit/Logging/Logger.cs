using System;
using System.Collections.Generic;

namespace Karambit.Logging
{
    public class Logger : IObservable<LogMessage>
    {
        #region Fields
        private List<string> blockedChannels = new List<string>() { "debug" };
        private List<IObserver<LogMessage>> observers = new List<IObserver<LogMessage>>();
        #endregion

        #region Methods        
        /// <summary>
        /// Blocks the specified channel from emitting log messages.
        /// </summary>
        /// <param name="channel">The channel.</param>
        public void Block(string channel) {
            if (!blockedChannels.Contains(channel))
                blockedChannels.Add(channel);
        }

        /// <summary>
        /// Unblocks the specified channel from emitting log messages.
        /// </summary>
        /// <param name="channel">The channel.</param>
        public void Unblock(string channel) {
            if (blockedChannels.Contains(channel))
                blockedChannels.Remove(channel);
        }

        /// <summary>
        /// Logs a message to the specified channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="msg">The message.</param>
        public void Log(LogLevel level, string channel, string msg) {
            // check if blocked
            if (blockedChannels.Contains(channel))
                return;

            // build
            string str = "[" + channel + "] " + msg;

            // write to console
            if (level == LogLevel.Error)
                WriteLineC(ConsoleColor.Red, str);
            else
                Console.WriteLine(str);

            // push to observers
            LogMessage message = new LogMessage(level, channel, msg);

            foreach (IObserver<LogMessage> observer in observers)
                observer.OnNext(message);
        }

        /// <summary>
        /// Subscribes the specified observer to outgoing log messages.
        /// </summary>
        /// <param name="observer">The observer.</param>
        /// <returns></returns>
        public IDisposable Subscribe(IObserver<LogMessage> observer) {
            observers.Add(observer);
            return null;
        }

        /// <summary>
        /// Writes the coloured line to the console.
        /// </summary>
        /// <param name="clr">The color.</param>
        /// <param name="msg">The message.</param>
        private static void WriteLineC(ConsoleColor clr, string msg) {
            Console.ForegroundColor = clr;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
        #endregion
    }
}
