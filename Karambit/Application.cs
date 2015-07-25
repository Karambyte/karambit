using Karambit.Logging;
using System;

namespace Karambit
{
    public delegate void StartedEventHandler(object sender, EventArgs e);
    public delegate void StoppedEventHandler(object sender, EventArgs e);

    public abstract class Application : IApplication
    {
        #region Fields
        private Deployment deployment = Deployment.Release;
        protected string name = "untitledapp";
        private bool running = false;

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
        #endregion

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

        #region Methods        
        /// <summary>
        /// Starts this application.
        /// </summary>
        public virtual void Start() {
            if (running)
                return;

            running = true;

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
        public Application() {
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