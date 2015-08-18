using System;
using System.Runtime.InteropServices;

namespace Karambit.Data.MySql
{
    public class MySqlRow
    {
        #region Fields
        private MySqlResult result;
        private object[] values;
        private string[] keys;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the values in this row.
        /// </summary>
        /// <value>The values.</value>
        public object[] Values {
            get {
                return values;
            }
        }

        /// <summary>
        /// Gets the keys for this row.
        /// </summary>
        /// <value>The keys.</value>
        public string[] Keys {
            get {
                if (keys == null) {
                    keys = new string[result.Fields.Length];

                    for (int i = 0; i < result.Fields.Length; i++)
                        keys[i] = result.Fields[i].Name;
                }

                return keys;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a row by fetching it from a result set.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        internal static MySqlRow FetchFromResult(MySqlResult result) {
            // fetch data
            IntPtr valueArr = MySqlNative.mysql_fetch_row(result.Handle);

            if (valueArr == IntPtr.Zero)
                return null;

            IntPtr lengthArr = MySqlNative.mysql_fetch_lengths(result.Handle);
            int numFields = (int)MySqlNative.mysql_num_fields(result.Handle);

            // create row
            MySqlRow row = new MySqlRow();
            row.result = result;
            row.values = new object[numFields];

            // lengths
            long[] length = new long[numFields];
            Marshal.Copy(lengthArr, length, 0, numFields);

            // values
            IntPtr[] valuePtrs = new IntPtr[numFields];
            Marshal.Copy(valueArr, valuePtrs, 0, numFields);

            for (int i = 0; i < numFields; i++) {
                row.values[i] = result.Fields[i].CastFrom(Marshal.PtrToStringAnsi(valuePtrs[i]));
            }

            return row;
        }
        #endregion

        #region Constructors
        private MySqlRow() {}
        #endregion
    }
}
