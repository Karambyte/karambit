using Karambit.Net;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Karambit.Web.HTTP
{
    public class HttpConnection : IHttpTransaction, IConnection
    {
        #region Fields
        private TcpClient client;
        private HttpServer server;
        private Guid id;
        private string address;
        private HttpProtocol protocol;
        private HttpWriter writer;
        private HttpReader reader;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the underlying client.
        /// </summary>
        /// <value>The client.</value>
        public TcpClient Client {
            get {
                return client;
            }
        }

        /// <summary>
        /// Gets the server.
        /// </summary>
        /// <value>The server.</value>
        public IServer Server {
            get {
                return server;
            }
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public Guid ID {
            get {
                return id;
            }
        }

        /// <summary>
        /// Gets the protocol used for communication.
        /// </summary>
        /// <value>The protocol.</value>
        public HttpProtocol Protocol {
            get {
                return protocol;
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
            // get stream
            NetworkStream stream = client.GetStream();

            // create reader/writer
            this.writer = new HttpWriter(stream, this);
            this.reader = new HttpReader(stream, this);

            // handle requests
            while (client.Connected) {
                Handle();
            }
        }

        private HttpRequest ReadRequest(HttpResponse res) {
            HttpRequest req = null;

            try {
                req = reader.ReadRequest();
            }
            catch (HttpException ex) {
                // create event
                ErrorEventArgs e = new ErrorEventArgs(ex);

                // invoke
                server.OnError(e);

                if (!e.Handled) {
                    // send
                    res.StatusCode = ex.StatusCode;

                    // send error report
                    if (Application.CurrentDeployment == Deployment.Production)
                        res.Write("<b><p>Request Error</p></b><pre>" + ex.Message + "</pre>");
                }
            }
            catch (Exception ex) {
                // status code
                res.StatusCode = HttpStatus.InternalServerError;

                // send error report
                try {
                    if (Application.CurrentDeployment == Deployment.Production)
                        res.Write("<b><p>Internal Error</p></b><pre>" + ex.ToString() + "</pre>");
                }
                catch (Exception) { }

                // close and return
                Close();
                return req;
            }

            Console.WriteLine("FINISHED occuredreq");
            return req;
        }

        private void WriteResponse(HttpResponse res) {

            // post-process
            if (res.Headers.Exists("Location"))
                res.StatusCode = HttpStatus.Found;

            res.Headers["Content-Length"] = res.Buffer.Length.ToString();
            res.Headers["Connection"] = "Keep-Alive";

            if (res.Server.Name != null)
                res.Headers["Server"] = res.Server.Name;

            // write response
            //try {
            writer.WriteResponse(res);
            //}
            //catch (Exception) { }
        }

        /// <summary>
        /// Handles the request.
        /// </summary>
        private void Handle() {
            // create response
            HttpResponse res = new HttpResponse(this);
            HttpRequest req = null;

            // read request
            if (!Utilities.InvokeAsync(delegate() { req = ReadRequest(res);}, 1000)) {
                // close and return
                Close();

                Console.WriteLine("timeout occuredreq");
                return;
            }

            // handle request
            if (req != null) {
                if (Application.CurrentDeployment == Deployment.Production) {
                    server.OnRequest(new RequestEventArgs(req, res));
                } else {
                    try {
                        server.OnRequest(new RequestEventArgs(req, res));
                    }
                    catch (Exception ex) {
                        // create event
                        ErrorEventArgs e = new ErrorEventArgs(new HttpException("Route exception", HttpStatus.InternalServerError, ex) { Path = req.Path, Method = req.Method});

                        // invoke
                        res.Clear();
                        res.StatusCode = HttpStatus.InternalServerError;
                        server.OnError(e);

                        if (!e.Handled) {
                            // internal error with request handler
                            res.Write("<pre>An internal server error occured!</pre>");
                        }
                    }
                }
            }

            // write
            if (!Utilities.InvokeAsync(delegate() { WriteResponse(res); }, 1000)) {
                // close and return
                Close();

                Console.WriteLine("timeout occuredres");
                return;
            }
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
        public HttpConnection(HttpServer server, Guid id, TcpClient client) {
            this.client = client;
            this.address = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
            this.server = server;
            this.id = id;
            this.protocol = HttpProtocol.HTTP;

            // process
            Utilities.InvokeAsync(Process);
        }
        #endregion
    }
}
