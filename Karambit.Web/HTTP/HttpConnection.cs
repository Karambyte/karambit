using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Karambit.Web.HTTP
{
    public class HttpConnection
    {
        #region Fields
        private TcpClient client;
        private HttpServer server;
        private ulong id;
        private Thread thread;
        private string address;

        private HttpStream stream;
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
        /// Gets the stream.
        /// </summary>
        /// <value>The stream.</value>
        public HttpStream Stream {
            get {
                return stream;
            }
        }

        /// <summary>
        /// Gets the remote address.
        /// </summary>
        /// <value>The address.</value>
        public string Address {
            get {
                return address;
            }
        }
        #endregion

        #region Methods        
        /// <summary>
        /// Processes the incoming connection.
        /// </summary>
        private void Process() {
            // create stream
            this.stream = new HttpStream(this, client.GetStream());

            // handle requests
            while (client.Connected) {
                Handle();
            }
        }

        /// <summary>
        /// Handles the request.
        /// </summary>
        private void Handle() {
            // create response
            HttpResponse res = new HttpResponse(this);
            HttpRequest req = null;

            // read request
            try {
                req = stream.Read();
            } catch (HttpException ex) {
                // send
                res.StatusCode = ex.StatusCode;

                // send error report
                if (server.Deployment == Deployment.Production)
                    res.Write("<b><p>Request Error</p></b><pre>" + ex.Message + "</pre>");
            } catch (Exception ex) {
                // send error report
                try {
                    if (server.Deployment == Deployment.Production)
                        res.Write("<b><p>Internal Error</p></b><pre>" + ex.ToString() + "</pre>");
                }
                catch (Exception) { }

                // close and return
                Close();
                return;
            }

            // handle request
            if (req != null) {
                try {
                    server.OnRequest(new RequestEventArgs(req, res));
                }
                catch (Exception ex) {
                    // internal error with request handler
                    res.Clear();
                    res.StatusCode = HttpStatus.InternalServerError;

                    // send error report
                    if (server.Deployment == Deployment.Production)
                        res.Write("<b><p>Application Error</p></b><pre>" + ex.ToString() + "</pre>");
                }
            }

            // write
            stream.Write(res);

            // close
            Close();
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        public void Close() {
            try {
                client.Close();
            } catch (Exception) { }
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
            this.address = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
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
