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

        }
        #endregion
    }
}
