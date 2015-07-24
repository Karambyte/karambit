using Karambit.Logging;
using System;

namespace Karambit
{
    public delegate void StartedEventHandler(object sender, EventArgs e);
    public delegate void StoppedEventHandler(object sender, EventArgs e);

    public abstract class Application : IApplication
    {
        #region Fields
        protected Deployment deployment = Deployment.Release;
        protected Logger logger = null;
        protected string name = "untitledapp";

        private static IApplication currentApp;
        #endregion

        #region Properties        
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

        #region Events
        public event StartedEventHandler Started;
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

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>The logger.</value>
        public virtual Logger Logger {
            get {
                return logger;
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public virtual string Name {
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
        #endregion

        #region Methods        
        /// <summary>
        /// Starts this application.
        /// </summary>
        public virtual void Start() {
            // invoke event
            OnStarted();
        }

        /// <summary>
        /// Stops this application.
        /// </summary>
        public virtual void Stop() {
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
                app.Stop();
            };

            // loop
            while (true)
                System.Threading.Thread.Sleep(0);
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        public Application() {
            this.logger = new Logger();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public Application(string name)
            : this() {
            this.name = name;
        }
        #endregion
    }
}