using Karambit.Web.HTTP;
using System;

namespace Karambit.Web
{
    public class Request : HttpRequest
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors
        public Request(HttpRequest req)
            : base(req.Connection) {
                this.headers = req.Headers;
                this.method = req.Method;
                this.path = req.Path;
                this.parameters = req.Parameters;
                this.version = req.Version;
        }
        #endregion
    }
}
