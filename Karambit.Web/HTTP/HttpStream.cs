using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace Karambit.Web.HTTP
{
    public class HttpStream
    {
        #region Fields
        private Stream stream;
        private StreamWriter writer;
        private StreamReader reader;
        private IHttpSource source;

        private static Dictionary<HttpStatus, string> statusStrings;
        #endregion

        #region Methods        
        /// <summary>
        /// Writes the specified response to the stream.
        /// </summary>
        /// <param name="res">The response.</param>
        public void WriteResponse(HttpResponse res) {
            // status code
            HttpStatus status = res.StatusCode;

            // override for location redirects
            if (res.Headers.ContainsKey("location"))
                status = HttpStatus.Found;

            // response
            writer.WriteLine(res.Version + " " + (int)status + " " + statusStrings[status]);

            // headers
            foreach (KeyValuePair<string, string> header in res.Headers) {
                writer.WriteLine(header.Key + ": " + header.Value);
            }

            // default headers
            writer.WriteLine("content-length: " + res.Buffer.Length);

            if (res.Server.Name != null)
                writer.WriteLine("server: " + res.Server.Name);

            // eoh
            writer.WriteLine("");

            // flush everything to stream
            writer.Flush();

            // body
            if (res.Buffer.Length > 0) {
                byte[] data = res.Buffer.ToArray();
                stream.Write(data, 0, data.Length);
            }
        }

        /// <summary>
        /// Writes the specified request..
        /// </summary>
        /// <param name="req">The req.</param>
        public void WriteRequest(HttpRequest req) {
            // request line
            string path = HttpUtilities.EncodeURL(req.Path);

            if (req.Parameters.Count > 0) {
                path += "?";

                // build parameters
                List<string> parameters = new List<string>();

                foreach(KeyValuePair<string, string> param in req.Parameters)
                    parameters.Add(string.Join("=", new string[]{HttpUtilities.EncodeURL(param.Key), HttpUtilities.EncodeURL(param.Value)}));

                // append parameters
                path += string.Join("&", parameters);
            }

            writer.WriteLine(req.Method.ToString() + " " + path + " HTTP/1.1");

            // headers
            foreach (KeyValuePair<string, string> header in req.Headers) {
                writer.WriteLine(header.Key.ToLower() + ": " + header.Value);
            }

            writer.WriteLine("content-length: 0");
            writer.WriteLine();

            // flush everything to stream
            writer.Flush();

            // body
            /// TODO: body
        }

        /// <summary>
        /// Reads a request from the stream.
        /// </summary>
        /// <returns>A request.</returns>
        public HttpRequest ReadRequest() {
            // request line
            string[] requestLine = reader.ReadLine().Split(' ');

            // check
            if (requestLine.Length != 3)
                throw new HttpException("The request line is invalid", HttpStatus.BadRequest) { Path = null };

            // create request
            HttpRequest req = new HttpRequest(source);

            // method
            bool validMethod = false;

            foreach (Enum e in Enum.GetValues(typeof(HttpMethod))) {
                if (e.ToString().ToLower() == requestLine[0].ToLower()) {
                    req.Method = (HttpMethod)e;
                    validMethod = true;
                    break;
                }
            }

            if (!validMethod)
                throw new HttpException("The method is not supported", HttpStatus.BadRequest) { Path = null };

            // version
            if (requestLine[2] != "HTTP/1.1")
                throw new HttpException("The version is invalid", HttpStatus.BadRequest) { Path = req.Path, Method = req.Method };

            req.Version = requestLine[2];

            // path
            string path = requestLine[1];

            if (path.IndexOf('?') > -1) {
                // split at question mark
                string[] splitPath = path.Split('?');

                // check length
                if (splitPath.Length != 2)
                    throw new HttpException("The path format is invalid", HttpStatus.BadRequest) { Path = path, Method = req.Method };

                // path
                req.Path = HttpUtilities.DecodeURL(splitPath[0]);

                // query parameters
                string[] splitParams = splitPath[1].Split('&');

                foreach (string param in splitParams) {
                    // parameter
                    string[] paramSplit = param.Split('=');
                    
                    if (paramSplit.Length != 2)
                        throw new HttpException("The path format is invalid", HttpStatus.BadRequest) { Path = req.Path, Method = req.Method };

                    string key = paramSplit[0];
                    string value = paramSplit[1];

                    // parameters
                    req.Parameters.Add(key, value);
                }
            } else {
                req.Path = HttpUtilities.DecodeURL(path);
            }

            // headers
            req.Headers = ReadHeaders();

            return req;
        }

        /// <summary>
        /// Reads the response from the stream.
        /// </summary>
        /// <returns></returns>
        public HttpResponse ReadResponse() {
            // response line
            string[] responseLine = reader.ReadLine().Split(' ');

            if (responseLine.Length < 3)
                throw new HttpException("The response line is invalid", HttpStatus.InternalServerError);

            // create response
            HttpResponse res = new HttpResponse(source);
            res.Version = responseLine[0];

            // check version
            if (res.Version != "HTTP/1.1")
                throw new HttpException("The version is invalid", HttpStatus.InternalServerError);

            // status code
            int statusCode = -1;

            if (!int.TryParse(responseLine[1], out statusCode))
                throw new HttpException("The status code is invalid", HttpStatus.InternalServerError);

            res.StatusCode = (HttpStatus)statusCode;

            // headers
            res.Headers = ReadHeaders();

            Application.Logger.Log(Logging.LogLevel.Information, "debug", "read " + res.Headers.Count + "headers");

            // body
            Application.Logger.Log(Logging.LogLevel.Information, "debug", "content length: " + (res.Headers.ContainsKey("content-length") ? "yes" : "no"));
            if (res.Headers.ContainsKey("content-length")) {
                // get length
                int contentLength = -1;

                if (!int.TryParse(res.Headers["content-length"], out contentLength))
                    throw new HttpException("The content length header is invalid", HttpStatus.InternalServerError);
                Application.Logger.Log(Logging.LogLevel.Information, "debug", "content length: " + contentLength);

                // ignore empty bodies
                if (contentLength > 0) {
                    // read
                    byte[] data = new byte[contentLength];
                    Application.Logger.Log(Logging.LogLevel.Information, "debug", "reading " + contentLength + " bytes");
                    stream.Read(data, 0, contentLength);
                    Application.Logger.Log(Logging.LogLevel.Information, "debug", "read " + contentLength + " bytes");

                    // buffer
                    res.Buffer = new MemoryStream(data);
                    Application.Logger.Log(Logging.LogLevel.Information, "debug", "created buffer for " + data.Length + " bytes");
                }
            }

            return res;
        }

        /// <summary>
        /// Reads a set of headers from the stream.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> ReadHeaders() {
            // dictionary
            Dictionary<string, string> headers = new Dictionary<string, string>();

            // headers
            string header = reader.ReadLine();

            while (header != "") {
                // split
                string key = "";
                string value = "";
                bool first = true;

                foreach (char c in header) {
                    if (first) {
                        if (c == ':') {
                            first = false;
                            continue;
                        }

                        key += c;
                    } else {
                        value += c;
                    }
                }

                // add
                headers.Add(key.ToLower(), value);

                // next
                header = reader.ReadLine();
            }

            return headers;
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpStream"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public HttpStream(IHttpSource source, Stream stream) {
            this.stream = stream;
            this.source = source;
            this.reader = new StreamReader(stream);
            this.writer = new StreamWriter(stream);
        }

        /// <summary>
        /// Initializes the <see cref="HttpStream"/> class and builds status strings.
        /// </summary>
        static HttpStream() {
            // status strings
            statusStrings = new Dictionary<HttpStatus, string>();

            // build
            foreach (Enum e in Enum.GetValues(typeof(HttpStatus))) {
                string enumString = e.ToString();
                List<string> outString = new List<string>();
                string curString = "";

                // iterate over the enum text
                foreach (char c in enumString) {
                    if (Char.IsUpper(c)) {
                        // join on single capitals to form one word
                        if (curString.Length == 1) {
                            curString += c;
                            continue;
                        }

                        // add word to output
                        if (curString != "")
                            outString.Add(curString);

                        // create new string from the character
                        curString = new string(new char[] { c });
                    } else {
                        curString += c;
                    }
                }

                // add current string if one exists
                if (curString != "") 
                    outString.Add(curString);

                // build the finished string, joining with spaces
                statusStrings.Add((HttpStatus)e, string.Join(" ", outString));
            }
        }
        #endregion
    }
}
