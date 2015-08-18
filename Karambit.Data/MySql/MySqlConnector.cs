using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Karambit.Data.MySql
{
    public class MySqlConnector : Connector
    {
        #region Fields
        private IntPtr handle;
        #endregion

        #region Properties
        internal IntPtr Handle {
            get {
                return handle;
            }
        }

        public string ErrorString {
            get {
                IntPtr str = MySqlNative.mysql_error(handle);
                return Marshal.PtrToStringAnsi(str);
            }
        }

        public uint ErrorNumber {
            get {
                return MySqlNative.mysql_errno(handle);
            }
        }

        public bool Error {
            get {
                return ErrorNumber != 0;
            }
        }

        public uint InsertId {
            get {
                return MySqlNative.mysql_insert_id(handle);
            }
        }
        #endregion

        #region Methods
        public override void Connect(string host, string username, string password) {
            Connect(host, username, password, null);
        }

        public override void Connect(string host, string username, string password, uint port) {
            Connect(host, username, password, null, port);
        }

        public override void Connect(string host, string username, string password, string db) {
            Connect(host, username, password, db, 3306);
        }

        public override void Connect(string host, string username, string password, string db, uint port) {
            MySqlNative.mysql_real_connect(handle, host, username, password, db, port, IntPtr.Zero, 0);
        }

        /// <summary>
        /// Executes the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="store">Whether or not to download the result set.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public MySqlResult Query(string query, bool store=true) {
            // list
            List<MySqlResult> results = new List<MySqlResult>();

            // query
            MySqlNative.mysql_real_query(handle, query, (uint)query.Length);

            // check error
            if (Error)
                throw new InvalidOperationException(ErrorString);

            Application.Logger.Log(Logging.LogLevel.Information, "mysql", query);

            while (true) {
                if (store)
                    results.Add(new MySqlResult(MySqlNative.mysql_store_result(handle)));
                else
                    results.Add(new MySqlResult(MySqlNative.mysql_store_result(handle)));

                break;
            }

            // exit
            return results[0];
        }

        /// <summary>
        /// Escapes the specified string, removing injection attempts.
        /// </summary>
        /// <param name="str">The query string.</param>
        /// <returns></returns>
        public string Escape(string str) {
            // output
            IntPtr to = Marshal.AllocHGlobal((str.Length * 2) + 1);

            // escape
            MySqlNative.mysql_real_escape_string(handle, to, str, (uint)str.Length);

            // convert
            string outStr = Marshal.PtrToStringAnsi(to);

            // fre output
            Marshal.FreeHGlobal(to);

            return outStr;
        }

        /// <summary>
        /// Disconnects from the MySql server.
        /// </summary>
        public override void Disconnect() {
            MySqlNative.mysql_close(handle);
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlConnector"/> class.
        /// </summary>
        public MySqlConnector() {
            this.handle = MySqlNative.mysql_init(IntPtr.Zero);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlConnector"/> class and connects to the specified host.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public MySqlConnector(string host, string username, string password) 
            : this() {
            Connect(host, username, password);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlConnector"/> class and connects to the specified host.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="port">The port.</param>
        public MySqlConnector(string host, string username, string password, uint port)
            : this() {
            Connect(host, username, password, null, port);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlConnector"/> class and connects to the specified host with the provided database.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="database">The database.</param>
        public MySqlConnector(string host, string username, string password, string database)
            : this() {
            Connect(host, username, password, database);
        }

        ~MySqlConnector() {
            Disconnect();
        }
        #endregion
    }
}
