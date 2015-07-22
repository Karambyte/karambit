using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace Karambit.Web.HTTP
{
    public class HttpStream
    {
        #region Fields
        private HttpConnection connection;
        private Stream stream;
        private StreamWriter writer;
        private StreamReader reader;

        private static Dictionary<HttpStatus, string> statusStrings;
        #endregion

        #region Methods        
        /// <summary>
        /// Writes the specified response to the stream.
        /// </summary>
        /// <param name="res">The response.</param>
        public void Write(HttpResponse res) {
            // status code
            HttpStatus status = res.StatusCode;

            // override for location redirects
            if (res.Headers.ContainsKey("Location"))
                status = HttpStatus.Found;

            // response
            writer.WriteLine("HTTP/1.1 " + (int)status + " " + statusStrings[status]);

            // headers
            foreach (KeyValuePair<string, string> header in res.Headers) {
                writer.WriteLine(header.Key + ": " + header.Value);
            }

            // default headers
            writer.WriteLine("Content-Length: " + res.Buffer.Length);
            writer.WriteLine("Connection: Keep-Alive");

            if (res.Server.Name != null)
                writer.WriteLine("Server: " + res.Server.Name);

            // eoh
            writer.WriteLine("");

            // flush everything to stream
            writer.Flush();

            // buffer
            byte[] data = res.Buffer.ToArray();
            stream.Write(data, 0, data.Length);
        }

        /// <summary>
        /// Reads a request from the stream.
        /// </summary>
        /// <returns>A request.</returns>
        public HttpRequest Read() {
            // request line
            string[] requestLine = reader.ReadLine().Split(' ');

            // check
            if (requestLine.Length != 3)
                throw new HttpException("The request line is invalid", HttpStatus.BadRequest) { Path = null };

            // create request
            HttpRequest req = new HttpRequest(connection);

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
                req.Path = path;
            }

            // version
            if (requestLine[2] != "HTTP/1.1")
                throw new HttpException("The version is invalid", HttpStatus.BadRequest) { Path = req.Path, Method = req.Method };

            req.Version = requestLine[2];

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
                req.Headers.Add(key, value);

                // next
                header = reader.ReadLine();
            }

            return req;
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpStream"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="stream">The stream.</param>
        public HttpStream(HttpConnection connection, Stream stream) {
            this.connection = connection;
            this.stream = stream;
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
