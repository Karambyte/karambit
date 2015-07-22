using System;

namespace Karambit.Web.HTTP
{
    public class ErrorEventArgs
    {
        #region Fields
        private bool handled;
        private HttpException exception;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ErrorEventArgs"/> is handled.
        /// </summary>
        /// <value><c>true</c> if handled; otherwise, <c>false</c>.</value>
        public bool Handled {
            get {
                return handled;
            }
            set {
                this.handled = value;
            }
        }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public HttpException Exception {
            get {
                return exception;
            }
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEventArgs"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public ErrorEventArgs(HttpException exception) {
            this.exception = exception;
            this.handled = false;
        }
        #endregion
    }
}
