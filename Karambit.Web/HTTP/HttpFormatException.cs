using System;

namespace Karambit.Web.HTTP
{
    public class HttpFormatException : Exception
    {
        #region Fields
        private IHttpTransaction transaction;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the HttpTransaction which received the malformed http message.
        /// </summary>
        /// <value>The transaction.</value>
        public IHttpTransaction Transaction {
            get {
                return transaction;
            }
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpFormatException"/> class.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        public HttpFormatException(IHttpTransaction transaction) 
            : base() {
            this.transaction = transaction;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpFormatException"/> class.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="message">The message.</param>
        public HttpFormatException(IHttpTransaction transaction, string message) 
            : base(message) {
            this.transaction = transaction; 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpFormatException"/> class.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public HttpFormatException(IHttpTransaction transaction, string message, Exception innerException) 
            : base(message, innerException) {
            this.transaction = transaction;
        }
        #endregion
    }
}
