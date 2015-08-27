using System;
using System.Collections.Generic;
using System.IO;

namespace Karambit.Web.HTTP
{
    public class HttpWriter
    {
        #region Fields
        private StreamWriter writer;
        private IHttpTransaction transaction;
        #endregion

        #region Methods        
        /// <summary>
        /// Writes the header to the stream.
        /// </summary>
        /// <param name="header">The header.</param>
        public void WriteHeader(HttpHeader header) {
            writer.WriteLine(header.Name + ": " + header.Value);
            writer.Flush();
        }

        /// <summary>
        /// Writes the collection of HTTP headers to the stream.
        /// </summary>
        /// <param name="headers">The headers.</param>
        public void WriteHeaders(HttpHeaderCollection headers) {
            foreach (HttpHeader header in headers)
                WriteHeader(header);
        }

        /// <summary>
        /// Writes the status line for the given status.
        /// </summary>
        /// <param name="status">The status.</param>
        public void WriteStatusLine(HttpStatus status) {
            writer.WriteLine("HTTP/1.1 " + (int)status + " " + ResourcesProcessor.StatusStrings[(int)status]);
            writer.Flush();
        }

        /// <summary>
        /// Writes the HTTP response to the stream.
        /// </summary>
        /// <param name="res">The resource.</param>
        public void WriteResponse(HttpResponse res) {
            // status line
            WriteStatusLine(res.StatusCode);

            // headers
            WriteHeaders(res.Headers);

            // end of headers
            writer.WriteLine();
            writer.Flush();

            // data
            if (res.Buffer.Length > 0) {
                byte[] data = res.Buffer.ToArray();
                writer.BaseStream.Write(data, 0, data.Length);
            }
        }

        /// <summary>
        /// Writes the HTTP request line to the stream.
        /// </summary>
        /// <param name="requestLine">The request line.</param>
        public void WriteRequestLine(HttpRequestLine requestLine) {
            // method
            writer.Write(requestLine.Method.ToString().ToUpper() + " ");

            // path
            writer.Write(HttpUtilities.EncodeString(requestLine.Path) + " ");

            // query
            if (requestLine.Query.Count > 0) {
                // query parameters start
                writer.Write("?");

                // pair up
                string[] pairs = new string[requestLine.Query.Count];

                int i = 0;
                foreach (KeyValuePair<string, string> kv in requestLine.Query) {
                    pairs[i] = HttpUtilities.EncodeString(kv.Key) + "=" + HttpUtilities.EncodeString(kv.Value);
                    i++;
                }

                // join pairs
                writer.Write(string.Join("&", pairs));
            }

            // version
            writer.WriteLine(requestLine.Version);

            // flush
            writer.Flush();
        }

        /// <summary>
        /// Writes the request.
        /// </summary>
        /// <param name="req">The request.</param>
        public void WriteRequest(HttpRequest req) {
            // request line
            WriteRequestLine(new HttpRequestLine() { Method = req.Method, Path = req.Path, Query = req.Query, Version = "HTTP/1.1" });

            // headers
            foreach (HttpHeader header in req.Headers) {
                WriteHeader(header);
            }

            // end of headers
            writer.WriteLine();
            writer.Flush();

            // data
            //if (req.Buffer.Length > 0) {
            //    byte[] data = req.Buffer.ToArray();
            //    writer.BaseStream.Write(data, 0, data.Length);
            //}
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpWriter"/> class with the provided stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="transaction">The transaction.</param>
        public HttpWriter(Stream stream, IHttpTransaction transaction) {
            this.writer = new StreamWriter(stream);
            this.transaction = transaction;
        }
        #endregion
    }
}
