using System;
using System.Runtime.InteropServices;

namespace Karambit.Data.MySql
{
    internal class MySqlNative
    {
        [DllImport("libmysql.dll")]
        internal static extern IntPtr mysql_init(IntPtr mysql);

        [DllImport("libmysql.dll")]
        internal static extern void mysql_close(IntPtr mysql);

        [DllImport("libmysql.dll", CharSet = CharSet.Ansi)]
        internal static extern IntPtr mysql_real_connect(IntPtr mysql, string host, string user, string password, string db, uint port, IntPtr socket, uint flags);

        [DllImport("libmysql.dll")]
        internal static extern IntPtr mysql_error(IntPtr mysql);

        [DllImport("libmysql.dll")]
        internal static extern uint mysql_errno(IntPtr mysql);

        [DllImport("libmysql.dll")]
        internal static extern uint mysql_insert_id(IntPtr mysql);

        [DllImport("libmysql.dll")]
        internal static extern int mysql_real_query(IntPtr mysql, string statement, uint length);

        [DllImport("libmysql.dll")]
        internal static extern IntPtr mysql_use_result(IntPtr mysql);

        [DllImport("libmysql.dll")]
        internal static extern IntPtr mysql_store_result(IntPtr mysql);

        [DllImport("libmysql.dll")]
        internal static extern void mysql_free_result(IntPtr result);

        [DllImport("libmysql.dll")]
        internal static extern IntPtr mysql_fetch_row(IntPtr result);

        [DllImport("libmysql.dll")]
        internal static extern IntPtr mysql_fetch_lengths(IntPtr result);

        [DllImport("libmysql.dll")]
        internal static extern uint mysql_num_rows(IntPtr result);

        [DllImport("libmysql.dll")]
        internal static extern uint mysql_num_fields(IntPtr result);

        [DllImport("libmysql.dll")]
        internal static extern IntPtr mysql_fetch_fields(IntPtr result);

        [DllImport("libmysql.dll")]
        internal static extern uint mysql_real_escape_string(IntPtr mysql, IntPtr to, string from, uint length);

        [DllImport("libmysql.dll")]
        internal static extern int mysql_next_result(IntPtr mysql);
    }
}
