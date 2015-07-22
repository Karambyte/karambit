using Karambit.Web.HTTP;
using System;
using System.Reflection;

namespace Karambit.Web
{
    public class ApplicationRoute
    {
        #region Fields
        private ApplicationRouteAttribute attribute;
        private MethodInfo method;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the assembly which the route resides in.
        /// </summary>
        /// <value>The assembly.</value>
        public Assembly Assembly {
            get {
                return method.Module.Assembly;  
            }
        }

        /// <summary>
        /// Gets the method.
        /// </summary>
        /// <value>The method.</value>
        public MethodInfo Function {
            get {
                return method;
            }
        }

        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <value>The attribute.</value>
        public ApplicationRouteAttribute Attribute {
            get {
                return attribute;
            }
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path {
            get {
                return attribute.Path;
            }
        }

        /// <summary>
        /// Gets the method.
        /// </summary>
        /// <value>The method.</value>
        public HttpMethod Method {
            get {
                return attribute.Method;
            }
        }

        /// <summary>
        /// Gets the host.
        /// </summary>
        /// <value>The host.</value>
        public string Host {
            get {
                return attribute.Host;
            }
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationRoute"/> class.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="method">The method.</param>
        public ApplicationRoute(ApplicationRouteAttribute attribute, MethodInfo method) {
            this.attribute = attribute;
            this.method = method;
        }
        #endregion
    }
}
