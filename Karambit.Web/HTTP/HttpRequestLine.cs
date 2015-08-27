using System;
using System.Collections.Generic;

namespace Karambit.Web.HTTP
{
    public class HttpRequestLine
    {
        public string Version;
        public string Path;
        public HttpMethod Method;
        public Dictionary<string, string> Query = new Dictionary<string, string>();
    }
}
