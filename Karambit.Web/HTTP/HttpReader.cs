using System;
using System.Collections.Generic;
using System.IO;

namespace Karambit.Web.HTTP
{
    public class HttpReader
    {
        #region Fields
        private StreamReader reader;
        private IHttpTransaction transaction;
        #endregion

        #region Methods        
        /// <summary>
        /// Reads the HTTP header from the stream.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpException">The header format is invalid</exception>
        public HttpHeader ReadHeader() {
            // read line
            string line = reader.ReadLine();

            if (line == "")
                return null;

            // validate
            int colon = line.IndexOf(':');

            if (colon == -1 || colon + 1 > line.Length)
                throw new HttpException("The header format is invalid", HttpStatus.BadRequest);

            // process
            return new HttpHeader(
                line.Substring(0, colon),
                line.Substring(colon + 1)
            );
        }

        /// <summary>
        /// Reads a collection of HTTP headers from the stream.
        /// </summary>
        /// <returns></returns>
        public HttpHeaderCollection ReadHeaders() {
            List<HttpHeader> headers = new List<HttpHeader>();

            // read header
            HttpHeader header = ReadHeader();

            while (header != null) {
                // add
                headers.Add(header);

                // next
                header = ReadHeader();
            }

            return new HttpHeaderCollection(headers.ToArray());
        }

        /// <summary>
        /// Reads a HTTP request line from the stream.
        /// </summary>
        /// <returns></returns>
        public HttpRequestLine ReadRequestLine() {
            // create request line
            HttpRequestLine requestLine = new HttpRequestLine();

            // read line
            string line = reader.ReadLine();

            // split
            string[] components = line.Split(' ');

            if (components.Length != 3)
                throw new HttpException("The request line format is invalid", HttpStatus.BadRequest);

            // version
            if (components[2].ToUpper() != "HTTP/1.1")
                throw new HttpException("The version is invalid", HttpStatus.BadRequest);

            requestLine.Version = components[2];

            // method
            bool validMethod = false;

            foreach (Enum e in Enum.GetValues(typeof(HttpMethod))) {
                if (e.ToString() == components[0].ToUpper()) {
                    requestLine.Method = (HttpMethod)e;
                    validMethod = true;
                    break;
                }
            }

            if (!validMethod)
                throw new HttpException("The request method is not supported", HttpStatus.BadRequest);

            // path
            string path = components[1];

            if (path.IndexOf('?') > -1) {
                // split at question mark
                string[] splitPath = path.Split('?');

                // check length
                if (splitPath.Length != 2)
                    throw new HttpException("The path format is invalid", HttpStatus.BadRequest);

                // path
                requestLine.Path = HttpUtilities.DecodeString(splitPath[0]);

                // query parameters
                string[] splitParams = splitPath[1].Split('&');

                foreach (string param in splitParams) {
                    // parameter
                    string[] paramSplit = param.Split('=');

                    if (paramSplit.Length != 2)
                        throw new HttpException("The path format is invalid", HttpStatus.BadRequest);

                    string key = paramSplit[0];
                    string value = paramSplit[1];

                    // parameters
                    requestLine.Query.Add(key, value);
                }
            } else {
                requestLine.Path = HttpUtilities.DecodeString(path);
            }

            return requestLine;
        }

        /// <summary>
        /// Reads the HTTP request from the stream with the provided stream.
        /// </summary>
        /// <returns></returns>
        public HttpRequest ReadRequest() {
            // request line
            HttpRequestLine reqLine = ReadRequestLine();

            // create request
            HttpRequest req = new HttpRequest(transaction);
            req.Path = reqLine.Path;
            req.Method = reqLine.Method;
            req.Query = reqLine.Query;

            // headers
            req.Headers = ReadHeaders();

            // body
            object contentLength = req.Headers["Content-Length"];

            if (contentLength != null) {
                byte[] data = new byte[(int)contentLength];
                reader.BaseStream.Read(data, 0, (int)contentLength);
            }

            return req;
        }

        /// <summary>
        /// Reads the status line from 
        /// </summary>
        /// <returns></returns>
        public HttpStatus ReadStatusLine() {
            // status line
            string statusLine = reader.ReadLine();
            string[] statusData = statusLine.Split(' ');

            // validate status line
            if (statusData.Length < 3)
                throw new HttpException("The status line format is invalid", HttpStatus.InternalServerError);

            // version
            if (statusData[0].ToUpper() != "HTTP/1.1")
                throw new HttpException("The HTTP version is unsupported", HttpStatus.InternalServerError);

            // status code
            int statusCode = -1;

            if (!int.TryParse(statusData[1], out statusCode))
                throw new HttpException("The status line format is invalid", HttpStatus.InternalServerError);

            return (HttpStatus)statusCode;
        }

        /// <summary>
        /// Reads the response.
        /// </summary>
        /// <returns></returns>
        public HttpResponse ReadResponse() {
            // create response
            HttpResponse response = new HttpResponse(transaction);
            response.StatusCode = ReadStatusLine();

            // headers
            response.Headers = ReadHeaders();

            return response;
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpReader"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="transaction">The transaction.</param>
        public HttpReader(Stream stream, IHttpTransaction transaction) {
            this.reader = new StreamReader(stream);
            this.transaction = transaction;
        }
        #endregion
    }
}
