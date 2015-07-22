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

        private NetworkStream stream;
        private StreamWriter writer;
        private StreamReader reader;
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

        /// <summary>
        /// Gets the writer.
        /// </summary>
        /// <value>The writer.</value>
        internal StreamWriter Writer {
            get {
                return writer;
            }
        }

        /// <summary>
        /// Gets the internal reader.
        /// </summary>
        /// <value>The reader.</value>
        internal StreamReader Reader {
            get {
                return reader;
            }
        }

        /// <summary>
        /// Gets the stream.
        /// </summary>
        /// <value>The stream.</value>
        internal Stream Stream {
            get {
                return stream;
            }
        }
        #endregion

        #region Methods        
        /// <summary>
        /// Processes the incoming connection.
        /// </summary>
        private void Process() {
            // streams
            this.stream = client.GetStream();
            this.reader = new StreamReader(this.stream);
            this.writer = new StreamWriter(this.stream);

            // handle requests
            while (client.Connected) {
                Handle();
            }
        }

        /// <summary>
        /// Handles the request.
        /// </summary>
        private void Handle() {
            // requests/response
            HttpRequest req = new HttpRequest(this);
            HttpResponse res = new HttpResponse(this);

            // request line
            string strReqLine = reader.ReadLine();
            string[] requestLine = strReqLine.Split(' ');

            if (requestLine.Length != 3) {
                res.Send(HttpStatus.BadRequest);
                return;
            }

            Console.WriteLine(strReqLine);
            res.Send(HttpStatus.OK);
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        public void Close() {
            client.Close();
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpConnection"/> class.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="client">The client.</param>
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
