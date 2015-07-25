using Karambit.Web.HTTP;
using System;

namespace Karambit.Web
{
    public class Response : HttpResponse
    {
        #region Fields
        #endregion

        #region Constructors
        public Response(HttpResponse res) 
            : base(res.Connection) {
                this.headers = res.Headers;
                this.status = res.StatusCode;
                this.buffer = res.Buffer;
        }
        #endregion
    }
}
