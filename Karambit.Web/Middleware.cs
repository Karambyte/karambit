using System;
using System.Reflection;

namespace Karambit.Web
{
    public class Middleware
    {
        #region Fields
        private MethodInfo method;
        private MiddlewareAttribute attribute;
        #endregion

        #region Methods
        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <value>The attribute.</value>
        public MiddlewareAttribute Attribute {
            get {
                return attribute;
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
        /// Gets the assembly which the route resides in.
        /// </summary>
        /// <value>The assembly.</value>
        public Assembly Assembly {
            get {
                return method.Module.Assembly;
            }
        }
        #endregion

        #region Constructors
        public Middleware(MiddlewareAttribute attribute, MethodInfo method) {
            this.attribute = attribute;
            this.method = method;
        }
        #endregion
    }
}
