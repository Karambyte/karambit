using Karambit.Web.HTTP;
using System;

namespace Karambit.Web
{
    public class Response : HttpResponse
    {
        #region Fields
        private HttpResponse res;
        #endregion

        #region Constructors
        public Response(HttpResponse res) 
            : base(res.Connection) {
        }
        #endregion
    }
}
