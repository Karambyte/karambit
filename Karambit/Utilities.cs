using System;

namespace Karambit
{
    public delegate void MethodInvoker();

    public static class Utilities
    {
        #region Methods
        public static void InvokeAsync(MethodInvoker method) {
            method.BeginInvoke(null, null);
        }

        public static bool InvokeAsync(MethodInvoker method, int timeout) {
            if (timeout < 0)
                throw new InvalidOperationException("The timeout cannot be less than zero");

            // invoke
            IAsyncResult res = method.BeginInvoke(null, null);

            // timeout
            return res.AsyncWaitHandle.WaitOne(timeout);
        }

        /// <summary>
        /// Dynamically parses the string based on the requested type.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool DynamicParse(string str, Type type, out object value) {
            if (type == typeof(string)) {
                value = str; return true;
            }
            else if (type == typeof(short)) {
                short val = default(short); if (short.TryParse(str, out val)) { value = val; return true; }
            }
            else if (type == typeof(ushort)) {
                ushort val = default(ushort); if (ushort.TryParse(str, out val)) { value = val; return true; }
            }
            else if (type == typeof(int)) {
                int val = default(int); if (int.TryParse(str, out val)) { value = val; return true; }
            }
            else if (type == typeof(uint)) {
                uint val = default(uint); if (uint.TryParse(str, out val)) { value = val; return true; }
            }
            else if (type == typeof(long)) {
                long val = default(long); if (long.TryParse(str, out val)) { value = val; return true; }
            }
            else if (type == typeof(ulong)) {
                ulong val = default(ulong); if (ulong.TryParse(str, out val)) { value = val; return true; }
            }
            else if (type == typeof(float)) {
                float val = default(float); if (float.TryParse(str, out val)) { value = val; return true; }
            }
            else if (type == typeof(bool)) {
                bool val = default(bool); if (bool.TryParse(str, out val)) { value = val; return true; }
            }
            else if (type == typeof(double)) {
                double val = default(double); if (double.TryParse(str, out val)) { value = val; return true; }
            }
            else if (type == typeof(byte)) {
                byte val = default(byte); if (byte.TryParse(str, out val)) { value = val; return true; }
            }
            else if (type == typeof(sbyte)) {
                sbyte val = default(sbyte); if (sbyte.TryParse(str, out val)) { value = val; return true; }
            }
            else if (type == typeof(Guid)) {
                Guid val = default(Guid); if (Guid.TryParse(str, out val)) { value = val; return true; }
            }

            value = null;
            return false;
        }
        #endregion
    }
}
