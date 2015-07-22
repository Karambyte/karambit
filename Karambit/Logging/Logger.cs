using System;

namespace Karambit.Logging
{
    public class Logger
    {
        #region Methods        
        /// <summary>
        /// Logs a message to the specified channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="msg">The message.</param>
        public void Log(LogLevel level, string channel, string msg) {
            string str = "[" + channel + "] " + msg;

            // write to console
            if (level == LogLevel.Error)
                WriteLineC(ConsoleColor.Red, str);
            else
                Console.WriteLine(str);
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
