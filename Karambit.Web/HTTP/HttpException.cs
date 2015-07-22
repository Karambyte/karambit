﻿using System;

namespace Karambit.Web.HTTP
{
    public class HttpException : Exception
    {
        #region Fields
        private HttpStatus status;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the status code that reperesents the error.
        /// </summary>
        /// <value>The status code.</value>
        public HttpStatus StatusCode {
            get {
                return status;
            }
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="code">The code.</param>
        public HttpException(string message, HttpStatus code) 
            : base(message) {
            this.status = code;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="code">The code.</param>
        /// <param name="innerException">The inner exception.</param>
        public HttpException(string message, HttpStatus code, Exception innerException) 
            : base(message, innerException) {
            this.status = code;
        }
        #endregion
    }
}
