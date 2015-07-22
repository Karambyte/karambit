using Karambit.Serialization;
using Karambit.Web.HTTP;
using Karambit.Web.Serialization;
using System;

namespace Karambit.Web
{
    public class Application
    {
        #region Constants
        private const string DefaultName = "Untitled Application";
        #endregion

        #region Fields
        private HttpServer server;
        private string name;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name {
            get {
                return name;
            }
        }

        /// <summary>
        /// Gets or sets the deployment.
        /// </summary>
        /// <value>The deployment.</value>
        public Deployment Deployment {
            get {
                return server.Deployment;
            }
            set {
                this.server.Deployment = value;

                // use tidy JSON in production
                if (this.server.Serializer is JSONSerializer)
                    ((JSONSerializer)this.server.Serializer).Format = (value == Deployment.Production) ? SerializerFormat.Tidy : SerializerFormat.Minimized;
            }
        }
        #endregion

        #region Methods        
        /// <summary>
        /// Handles the internal request.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RequestEventArgs"/> instance containing the event data.</param>
        private void HandleRequest(object sender, RequestEventArgs e) {
            Console.WriteLine(e.Request.Path);
        }

        /// <summary>
        /// Starts this application.
        /// </summary>
        public void Start() {
            server.Start();
        }

        /// <summary>
        /// Stops this application.
        /// </summary>
        public void Stop() {
            server.Stop();
        }
        #endregion

        #region Constructors                
        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        /// <param name="port">The port.</param>
        public Application(int port) {
            // server
            this.server = new HttpServer(port);
            this.server.Request += HandleRequest;
            this.server.Serializer = new JSONSerializer(SerializerFormat.Minimized);

            // default name.
            this.name = DefaultName;

            // default deployment
            Deployment = Deployment.Release;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <param name="name">The name.</param>
        public Application(int port, string name)
            : this(port) {
            this.name = name;
        }
        #endregion
    }
}
