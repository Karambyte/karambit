using Karambit.Logging;
using Karambit.Net;
using Karambit.Serialization;
using Karambit.Web.HTTP;
using Karambit.Web.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Karambit.Web
{
    public class WebApplication : Application
    {
        #region Fields
        private List<Route> routes = new List<Route>();
        private List<Middleware> middleware = new List<Middleware>();
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the deployment.
        /// </summary>
        /// <value>The deployment.</value>
        public override Deployment Deployment {
            set {
                base.Deployment = value;

                foreach (IServer server in servers) {
                    if (server is HttpServer) {
                        HttpServer httpServer = (HttpServer)server;
                        httpServer.Deployment = value;

                        if (httpServer.Serializer is JSONSerializer)
                            ((JSONSerializer)httpServer.Serializer).Format = (value == Deployment.Production) ? SerializerFormat.Tidy : SerializerFormat.Minimized;
                    }
                }
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
            Route route = null;

            // create request/response
            Request req = new Request(e.Request);
            Response res = new Response(e.Response);

            // name header
            e.Response.Headers["X-Karambit-App"] = name;

            // middleware
            foreach (Middleware mware in middleware) {
                // invoke middleware
                bool handled = (bool)mware.Function.Invoke(null, new object[] { req, res });

                // check if handed
                if (handled) {
                    if (Deployment == Deployment.Production)
                        Logger.Log(LogLevel.Information, "http", req.Method + " " + req.Path);
                    return;
                }
            }

            // find route
            foreach (Route r in routes) {
                if (r.Path == req.Path && r.Method == req.Method) {
                    route = r;
                    break;
                }
            }

            // check found
            if (route == null) {
                res.StatusCode = HttpStatus.NotFound;
                res.Write("Cannot " + req.Method + " " + req.Path);

                // log
                goto fail;
            }

            // parameters
            ParameterInfo[] parameters = route.Function.GetParameters();
            object[] parameterValues = new object[parameters.Length];

            // request and response
            parameterValues[0] = req;
            parameterValues[1] = res;

            // iterate
            for (int i = 2; i < parameters.Length; i++) {
                // get parameter
                ParameterInfo info = parameters[i];
                bool hasParameter = req.Parameters.ContainsKey(info.Name);

                if (!info.HasDefaultValue && !hasParameter) {
                    // missing parameter and no default value
                    res.StatusCode = HttpStatus.BadRequest;
                    res.Write("The parameter " + info.Name + " is missing from the request");

                    goto fail;
                } else if (info.HasDefaultValue && !hasParameter) {
                    // missing parameter has default value
                    parameterValues[i] = info.DefaultValue;
                } else {
                    // parameter exists
                    parameterValues[i] = req.Parameters[info.Name];
                }
            }

            // log
            if (Deployment == Deployment.Production)
                Logger.Log(LogLevel.Information, "http", req.Method + " " + req.Path);

            // invoke
            route.Function.Invoke(null, parameterValues);
            return;

            // fail message
            fail:
            if (Deployment == Deployment.Production)
                Logger.Log(LogLevel.Error, "http", req.Method + " " + req.Path);
        }

        /// <summary>
        /// Handles the error.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ErrorEventArgs"/> instance containing the event data.</param>
        private void HandleError(object sender, ErrorEventArgs e) {
            // log
            if (Deployment == Deployment.Production)
                Logger.Log(LogLevel.Error, "http", e.Exception.Method + " " + e.Exception.Path);
        }

        /// <summary>
        /// Adds all routes and middleware from the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public void Add(Assembly assembly) {
            // get all types
            foreach (Type t in assembly.GetTypes()) {
                // get all methods
                foreach (MethodInfo method in t.GetMethods()) {
                    // ignore non-static methods
                    if (!method.IsStatic)
                        continue;

                    Attribute routeAtt = method.GetCustomAttribute(typeof(RouteAttribute));
                    Attribute middlewareAtt = method.GetCustomAttribute(typeof(MiddlewareAttribute));
                    Attribute att = (routeAtt != null) ? routeAtt : ((middlewareAtt != null) ? middlewareAtt : null);
                    string attEntity = (routeAtt != null) ? "route" : ((middlewareAtt != null) ? "middleware" : null);
                    string methodEntity = t.Name + "." + method.Name;

                    // if one attribute is found
                    if (att != null) {
                        // get parameters
                        ParameterInfo[] parameters = method.GetParameters();

                        if ((routeAtt != null && parameters.Length < 2) || (middlewareAtt != null && parameters.Length != 2)) {
                            Log(LogLevel.Error, "The " + attEntity + " " + methodEntity + " requires at least 2 parameters");
                            continue;
                        } else if (parameters[0].ParameterType != typeof(Request)) {
                            Log(LogLevel.Error, "The " + attEntity + " " + methodEntity + " must have a Request object as it's first parameter");
                            continue;
                        } else if (parameters[1].ParameterType != typeof(Response)) {
                            Log(LogLevel.Error, "The " + attEntity + " " + methodEntity + " must have a Response object as it's second parameter");
                            continue;
                        } else if (middlewareAtt != null && method.ReturnType != typeof(bool)) {
                            Log(LogLevel.Error, "The " + attEntity + " " + methodEntity + " must return a boolean");
                            continue;
                        }

                        if (att is RouteAttribute)
                            routes.Add(new Route((RouteAttribute)att, method));
                        else
                            middleware.Add(new Middleware((MiddlewareAttribute)att, method));
                    }
                }
            }
        }

        /// <summary>
        /// Removes all reoutes and middleware from the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public void Remove(Assembly assembly) {
            List<Route> removeRoutes = new List<Route>();
            List<Middleware> removeMiddleware = new List<Middleware>();

            // find all routes with the specified assembly
            foreach (Route route in routes) {
                if (route.Assembly == assembly)
                    removeRoutes.Add(route);
            }

            // find all middleware with the specified assembly
            foreach (Middleware mware in middleware) {
                if (mware.Assembly == assembly)
                    removeMiddleware.Add(mware);
            }

            // remove all
            foreach (Route route in removeRoutes)
                routes.Remove(route);
            foreach (Middleware mware in removeMiddleware)
                middleware.Remove(mware);
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WebApplication"/> class.
        /// </summary>
        /// <param name="port">The port.</param>
        public WebApplication(int port) 
            : base() {
            // server
            HttpServer server = new HttpServer(port);
            server.Request += HandleRequest;
            server.Error += HandleError;
            server.Serializer = new JSONSerializer(SerializerFormat.Minimized);

            // attach
            Attach(server);

            // search for routes
            Add(Assembly.GetEntryAssembly());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApplication"/> class.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <param name="name">The name.</param>
        public WebApplication(int port, string name)
            : base(name) {
            // server
            HttpServer server = new HttpServer(port);
            server.Request += HandleRequest;
            server.Error += HandleError;
            server.Serializer = new JSONSerializer(SerializerFormat.Minimized);

            // attach
            Attach(server);

            // search for routes
            Add(Assembly.GetEntryAssembly());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApplication"/> class.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <param name="name">The name.</param>
        /// <param name="deployment">The deployment.</param>
        public WebApplication(int port, string name, Deployment deployment)
            : this(port, name) {
            this.Deployment = deployment;
        }
        #endregion
    }
}
