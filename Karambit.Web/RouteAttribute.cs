using Karambit.Web.HTTP;
using System;

namespace Karambit.Web
{
    public class RouteAttribute : Attribute
    {
        #region Fields
        private HttpMethod method;
        private string path;
        private string host;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the method.
        /// </summary>
        /// <value>
        /// The method.
        /// </value>
        public HttpMethod Method {
            get {
                return method;
            }
            set {
                this.method = value;
            }
        }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path {
            get {
                return path;
            }
            set {
                this.path = value;
            }
        }

        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>The host.</value>
        public string Host {
            get {
                return host;
            }
            set {
                this.host = value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RouteAttribute"/> class.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="path">The path.</param>
        public RouteAttribute(HttpMethod method, string path) {
            this.method = method;
            this.path = path;
            this.host = "*";
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RouteAttribute"/> class.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="path">The path.</param>
        /// <param name="host">The host.</param>
        public RouteAttribute(HttpMethod method, string path, string host) {
            this.method = method;
            this.path = path;
            this.host = host;
        }
        #endregion
    }
}
