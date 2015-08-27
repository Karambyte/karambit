using System;
using System.Collections;
using System.Collections.Generic;

namespace Karambit.Web.HTTP
{
    /// <summary>
    /// A class which represents a collection of headers.
    /// This class is thread-safe.
    /// </summary>
    public class HttpHeaderCollection : IEnumerable<HttpHeader>
    {
        #region Fields
        private List<HttpHeader> headers;
        #endregion

        #region Indexers        
        /// <summary>
        /// Gets or sets the header value for the provided name.
        /// </summary>
        /// <value>The value of the provided header, or null if none found</value>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public object this[string name] {
            get {
                return Get(name);
            } set {
                // remove old
                Remove(name);

                // add new
                Add(new HttpHeader(name, (string)value));
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the number of headers in this collection.
        /// </summary>
        /// <value>The count.</value>
        public int Count {
            get {
                return headers.Count;
            }
        }

        /// <summary>
        /// Gets the names.
        /// </summary>
        /// <value>The names.</value>
        public string[] Names {
            get {
                string[] names = new string[headers.Count];

                for (int i = 0; i < headers.Count; i++)
                    names[i] = headers[i].Name;

                return names;
            }
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>The values.</value>
        public object[] Values {
            get {
                object[] values = new string[headers.Count];

                for (int i = 0; i < headers.Count; i++)
                    values[i] = headers[i].Value;

                return values;
            }
        }
        #endregion

        #region Methods        
        /// <summary>
        /// Adds the specified header to the collection.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="value">The value.</param>
        public void Add(string header, string value) {
            Add(new HttpHeader(header, value));
        }

        /// <summary>
        /// Adds the specified header to the collection.
        /// </summary>
        /// <param name="header">The header.</param>
        public void Add(HttpHeader header) {
            lock (headers) {
                if (Exists(header.Name))
                    return;

                headers.Add(header);
            }
        }

        /// <summary>
        /// Adds the range of headers to the collection.
        /// </summary>
        /// <param name="headers">The headers.</param>
        public void AddRange(HttpHeader[] headers) {
            lock (headers) {
                foreach (HttpHeader header in this.headers) {
                    if (Exists(header.Name))
                        return;

                    this.headers.Add(header);
                }
            }
        }

        /// <summary>
        /// Gets the header by the specified name, or null if none found.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public HttpHeader Get(string name) {
            lock (headers) {
                foreach (HttpHeader header in headers) {
                    if (header.Name.ToLower() == name)
                        return header;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the header value for the specified name, or the default value if not found.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="def">The default.</param>
        /// <returns></returns>
        public object Get(string name, object def) {
            // get
            HttpHeader header = Get(name);

            return (header == null) ? def : header.Value;
        }

        /// <summary>
        /// Check if the header exists with the provided name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public bool Exists(string name) {
            lock (headers) {
                foreach (HttpHeader header in headers) {
                    if (header.Name.ToLower() == name)
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Removes the header with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        public void Remove(string name) {
            HttpHeader toRemove = Get(name);

            lock (headers) {
                if (toRemove != null)
                    headers.Remove(toRemove);
            }
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHeaderCollection"/> class with no headers.
        /// </summary>
        public HttpHeaderCollection() {
            this.headers = new List<HttpHeader>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHeaderCollection"/> class with a range of headers.
        /// </summary>
        /// <param name="headers">The headers.</param>
        public HttpHeaderCollection(HttpHeader[] headers) {
            this.headers = new List<HttpHeader>(headers);
        }
        #endregion

        public IEnumerator<HttpHeader> GetEnumerator() {
            return headers.GetEnumerator();
        }

        IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return headers.GetEnumerator();
        }
    }
}
