using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Karambit.Web
{
    public class HttpResponse
    {
        #region Fields
        private HttpConnection connection;
        private MemoryStream buffer;
        private HttpStatus status;

        private static Dictionary<HttpStatus, string> statusStrings;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the underlying connection.
        /// </summary>
        /// <value>The connection.</value>
        public HttpConnection Connection {
            get {
                return connection;
            }
        }

        /// <summary>
        /// Gets the server.
        /// </summary>
        /// <value>The server.</value>
        public HttpServer Server {
            get {
                return connection.Server;
            }
        }
        #endregion

        #region Methods
        public void Write(string str) {
            // get bytes
            byte[] bytes = Encoding.UTF8.GetBytes(str);

            // write
            buffer.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Sends the response and flushes the buffer.
        /// </summary>
        public void Send() {
            // response
            connection.Writer.WriteLine("HTTP/1.1 " + (int)status + " " + statusStrings[status]);
            connection.Writer.WriteLine("Content-Type: application/json");
            connection.Writer.WriteLine("Content-Length: " + buffer.Length);
            connection.Writer.WriteLine("");
            connection.Writer.Flush();
            
            // buffer
            byte[] data = buffer.ToArray();
            connection.Stream.Write(data, 0, data.Length);

            // close
            connection.Close();
        }

        /// <summary>
        /// Sends the response with the specified code and flushes the buffer.
        /// </summary>
        /// <param name="code">The code.</param>
        public void Send(HttpStatus code) {
            // write message if no data already wrote
            if (buffer.Position == 0)
                Write(statusStrings[code]);

            // set status code
            this.status = code;
            Console.WriteLine(statusStrings[code]);

            // end response
            Send();
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpResponse"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public HttpResponse(HttpConnection connection) {
            this.connection = connection;
            this.status = HttpStatus.OK;
            this.buffer = new MemoryStream();
        }

        /// <summary>
        /// Initializes the <see cref="HttpResponse"/> class and builds status strings.
        /// </summary>
        static HttpResponse() {
            // status strings
            statusStrings = new Dictionary<HttpStatus, string>();

            // build
            foreach (Enum e in Enum.GetValues(typeof(HttpStatus))) {
                string enumString = e.ToString();
                List<string> outString = new List<string>();
                string curString = "";

                foreach (char c in enumString) {
                    if (Char.IsUpper(c)) {
                        if (curString.Length == 1) {
                            curString += c;
                            continue;
                        }

                        if (curString != "")
                            outString.Add(curString);

                        curString = new string(new char[] { c });
                    } else {
                        curString += c;
                    }
                }

                if (curString != "") 
                    outString.Add(curString);

                statusStrings.Add((HttpStatus)e, string.Join(" ", outString));
            }
        }
        #endregion
    }
}
