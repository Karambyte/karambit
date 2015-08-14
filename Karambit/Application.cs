using Karambit.Logging;
using Karambit.Net;
using System;
using System.Collections.Generic;

namespace Karambit
{
	/// <summary>
	///  Started event handler.
	/// </summary>
    public delegate void StartedEventHandler(object sender, EventArgs e);

	/// <summary>
	/// Stopped event handler.
	/// </summary>
    public delegate void StoppedEventHandler(object sender, EventArgs e);

	/// <summary>
	/// An abstract class which represents the Karambit application framework.
	/// </summary>
    public abstract class Application : IApplication
    {
        #region Fields
        private Deployment deployment = Deployment.Release;
        private bool running = false;

		protected string name = "untitledapp";
		protected List<IServer> servers = new List<IServer>();

        private static Logger logger;
        private static IApplication currentApp;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the current logger.
        /// </summary>
        /// <value>The logger.</value>
        public static Logger Logger {
            get {
                if (logger == null)
                    logger = new Logger();
                return logger;
            }
        }

        /// <summary>
        /// Gets the deployment type.
        /// </summary>
        /// <value>The deployment.</value>
        public virtual Deployment Deployment {
            get {
                return deployment;
            }
            set {
                deployment = value;
            }
        }
        
        /// <summary>
        /// Gets a value indicating whether this <see cref="Application"/> is running.
        /// </summary>
        /// <value>
        ///   <c>true</c> if running; otherwise, <c>false</c>.
        /// </value>
        public bool Running {
            get {
                return running;
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name {
            get {
                return name;
            }
            set {
                this.name = value;
            }
        }

        /// <summary>
        /// Gets the currently executing application (if any).
        /// </summary>
        /// <value>The current app.</value>
        public static IApplication Current {
            get {
                return currentApp;
            }
        }

        /// <summary>
        /// Gets or sets the current deployment.
        /// When attempting to set the deployment when no application is running,
        /// this property will do nothing. Getting the deployment value will return
        /// release if no application is running.
        /// </summary>
        /// <value>The current deployment.</value>
        public static Deployment CurrentDeployment {
            get {
                if (currentApp == null)
                    return Deployment.Release;

                return currentApp.Deployment;
            } set {
                if (currentApp == null)
                    return;

                currentApp.Deployment = value;
            }
        }
        #endregion

        #region Events
		/// <summary>
		///  Occurs when the application has started.
		/// </summary>
        public event StartedEventHandler Started;

		/// <summary>
		/// Occurs when the application has stopped.
		/// </summary>
        public event StoppedEventHandler Stopped;

        /// <summary>
        /// Called when the application starts.
        /// </summary>
        protected virtual void OnStarted() {
            if (Started != null)
                Started(this, new EventArgs());
        }

        /// <summary>
        /// Called when the application stops.
        /// </summary>
        protected virtual void OnStopped() {
            if (Stopped != null)
                Stopped(this, new EventArgs());
        }
        #endregion

        #region Methods
        /// <summary>
        /// Starts this application.
        /// </summary>
        public virtual void Start() {
            if (running)
                return;

            running = true;

            // start servers
            foreach (IServer server in servers)
                server.Start();

            // invoke event
            OnStarted();
        }

        /// <summary>
        /// Stops this application.
        /// </summary>
        public virtual void Stop() {
            if (!running)
                return;

            running = false;

            // stop servers
            foreach (IServer server in servers)
                server.Stop();

            // invoke event
            OnStopped();
        }

        /// <summary>
        /// Logs a message to the application channel.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="msg">The message.</param>
        public virtual void Log(LogLevel level, string msg) {
            Logger.Log(level, name.ToLower(), msg);
        }

        /// <summary>
        /// Attaches the specified server to the application.
        /// </summary>
        /// <param name="server">The server.</param>
        public virtual void Attach(IServer server) {
            servers.Add(server);
        }

        /// <summary>
        /// Detaches the specified server from the application.
        /// </summary>
        /// <param name="server">The server.</param>
        public virtual void Detach(IServer server) {
            servers.Remove(server);
        }

        /// <summary>
        /// Runs the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="args">The arguments.</param>
        public static void Run(IApplication app, string[] args) {
            // deployment
            app.Deployment = (System.Diagnostics.Debugger.IsAttached) ? Deployment.Production : Deployment.Release;

            // start
            app.Start();

            // set current
            currentApp = app;

            // stop on process exit
            AppDomain.CurrentDomain.ProcessExit += delegate(object sender, EventArgs e) {
                if (app.Running)
                    app.Stop();
            };

            // loop
            while (true) {
                string command = Console.ReadLine();
                if (command == "quit" || command == "exit")
                    return;
                else if (command == "clear")
                    Console.Clear();
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        protected Application() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        protected Application(string name)
            : this() {
            this.name = name;
        }
        #endregion
    }
}