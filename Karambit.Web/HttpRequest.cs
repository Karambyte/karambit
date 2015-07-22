using System;

namespace Karambit.Web
{
    public class HttpRequest
    {
        #region Fields
        private HttpConnection connection;
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequest"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public HttpRequest(HttpConnection connection) {
            this.connection = connection;
        }
        #endregion
    }
}
