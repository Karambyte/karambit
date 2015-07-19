using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Karambit.Web
{
    public class HttpConnection
    {
        #region Fields
        private TcpClient client;
        private HttpServer server;
        private ulong id;
        private Thread thread;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the server.
        /// </summary>
        /// <value>The server.</value>
        public HttpServer Server {
            get {
                return server;
            }
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public ulong ID {
            get {
                return id;
            }
        }
        #endregion

        #region Methods
        private void Process() {
            StreamReader sr = new StreamReader(client.GetStream());

            string requestLine = sr.ReadLine();
            Console.WriteLine(requestLine);

            string stuff = "{\"jamie\":\"sucks\"}";
            byte[] stuffData = Encoding.UTF8.GetBytes(stuff);
            StreamWriter sw = new StreamWriter(client.GetStream());
            sw.WriteLine("HTTP/1.1 200 OK");
            sw.WriteLine("Content-Type: application/json");
            sw.WriteLine("Content-Length: " + stuffData.Length + "");
            sw.WriteLine();
            sw.Flush();
            try {
                sw.BaseStream.Write(stuffData, 0, stuffData.Length);
            }
            catch (Exception) { }
            client.Close();
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        public void Close() {
            client.Close();
        }
        #endregion

        #region Constructors
        public HttpConnection(HttpServer server, ulong id, TcpClient client) {
            this.client = client;
            this.server = server;
            this.id = id;

            // thread
            this.thread = new Thread(new ThreadStart(Process));
            this.thread.Name = "KHttpConn";
            this.thread.IsBackground = true;
            this.thread.Start();
        }
        #endregion
    }
}
