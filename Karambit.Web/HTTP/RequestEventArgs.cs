using System;

namespace Karambit.Web.HTTP
{
    public class RequestEventArgs : EventArgs
    {
        #region Fields
        private HttpRequest request;
        private HttpResponse response;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the request.
        /// </summary>
        /// <value>The request.</value>
        public HttpRequest Request {
            get {
                return request;
            }
        }

        /// <summary>
        /// Gets the response.
        /// </summary>
        /// <value>The response.</value>
        public HttpResponse Response {
            get {
                return response;
            }
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestEventArgs"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        public RequestEventArgs(HttpRequest request, HttpResponse response) {
            this.request = request;
            this.response = response;
        }
        #endregion
    }
}
