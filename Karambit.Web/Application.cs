﻿using Karambit.Logging;
using Karambit.Serialization;
using Karambit.Web.HTTP;
using Karambit.Web.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Karambit.Web
{
    public delegate void StartedEventHandler(object sender, EventArgs e);
    public delegate void StoppedEventHandler(object sender, EventArgs e);

    public class Application
    {
        #region Constants
        private const string DefaultName = "Untitled Application";
        #endregion

        #region Fields
        private HttpServer server;
        private string name;
        private List<ApplicationRoute> routes;
        private Logger logger;
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
        /// Gets the logger.
        /// </summary>
        /// <value>The logger.</value>
        public Logger Logger {
            get {
                return logger;
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
        /// Handles the internal request.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RequestEventArgs"/> instance containing the event data.</param>
        private void HandleRequest(object sender, RequestEventArgs e) {
            ApplicationRoute route = null;

            // find route
            foreach (ApplicationRoute r in routes) {
                if (r.Path == e.Request.Path && r.Method == e.Request.Method) {
                    route = r;
                    break;
                }
            }

            // check found
            if (route == null) {
                e.Response.StatusCode = HttpStatus.NotFound;
                e.Response.Write("Cannot " + e.Request.Method + " " + e.Request.Path);

                // log
                logger.Log(LogLevel.Error, "http", e.Request.Method + " " + e.Request.Path);
                return;
            }

            // log
            logger.Log(LogLevel.Information, "http", e.Request.Method + " " + e.Request.Path);

            // invoke
            route.Function.Invoke(null, new object[] { e.Request, e.Response });
        }

        /// <summary>
        /// Starts this application.
        /// </summary>
        public void Start() {
            server.Start();

            // invoke event
            OnStarted();
        }

        /// <summary>
        /// Stops this application.
        /// </summary>
        public void Stop() {
            server.Stop();

            // invoke event
            OnStopped();
        }

        /// <summary>
        /// Adds all routes from the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public void Add(Assembly assembly) {
            // get all types
            foreach (Type t in assembly.GetTypes()) {
                // get all methods
                foreach (MethodInfo method in t.GetMethods()) {
                    Attribute att = method.GetCustomAttribute(typeof(ApplicationRouteAttribute));

                    // if static and has route attribute, add
                    if (method.IsStatic && att != null) {
                        routes.Add(new ApplicationRoute((ApplicationRouteAttribute)att, method));
                    }
                }
            }
        }

        /// <summary>
        /// Removes all reoutes from the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public void Remove(Assembly assembly) {
            List<ApplicationRoute> removeRoutes = new List<ApplicationRoute>();

            // find all routes with the specified assembly
            foreach (ApplicationRoute route in routes) {
                if (route.Assembly == assembly)
                    removeRoutes.Add(route);
            }

            // remove all
            foreach (ApplicationRoute route in removeRoutes)
                routes.Remove(route);
        }

        /// <summary>
        /// Runs the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        public static void Run(Application app) {
            // start
            app.Start();

            // add stop handler
            AppDomain.CurrentDomain.ProcessExit += delegate(object sender, EventArgs e) {
                app.Stop();
            };

            // wait
            while (true)
                Console.ReadLine();
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

            // routes
            this.routes = new List<ApplicationRoute>();

            // default name.
            this.name = DefaultName;

            // default deployment
            Deployment = Deployment.Release;

            // default logger
            this.logger = new Logger();

            // search for routes
            Add(Assembly.GetEntryAssembly());
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

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <param name="name">The name.</param>
        /// <param name="logger">The logger.</param>
        public Application(int port, string name, Logger logger)
            : this(port, name) {
            this.logger = logger;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <param name="name">The name.</param>
        /// <param name="deployment">The deployment.</param>
        public Application(int port, string name, Deployment deployment)
            : this(port, name) {
            this.Deployment = deployment;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <param name="name">The name.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="deployment">The deployment.</param>
        public Application(int port, string name, Logger logger, Deployment deployment)
            : this(port, name, logger) {
            this.Deployment = deployment;
        }
        #endregion
    }
}
