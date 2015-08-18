using System;
using System.Security.Cryptography;
using System.Text;

namespace Karambit.Security
{
    public static class Hashing
    {
        #region Methods
        /// <summary>
        /// Computes the hash of the specified string and converts it into hexdecimal.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        [Obsolete("This hash method is insecure")]
        public static string MD5(string str) {
            MD5 crypt = System.Security.Cryptography.MD5.Create();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(str), 0, Encoding.UTF8.GetByteCount(str));
            foreach (byte theByte in crypto) {
                hash += theByte.ToString("x2");
            }
            return hash;
        }

        /// <summary>
        /// Computes the hash of the specified string and converts it into hexdecimal.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        [Obsolete("This hash method is insecure")]
        public static string SHA128(string str) {
            SHA1 crypt = System.Security.Cryptography.SHA1.Create();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(str), 0, Encoding.UTF8.GetByteCount(str));
            foreach (byte theByte in crypto) {
                hash += theByte.ToString("x2");
            }
            return hash;
        }

        /// <summary>
        /// Computes the hash of the specified string and converts it into hexdecimal.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string SHA256(string str) {
            SHA256 crypt = System.Security.Cryptography.SHA256.Create();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(str), 0, Encoding.UTF8.GetByteCount(str));
            foreach (byte theByte in crypto) {
                hash += theByte.ToString("x2");
            }
            return hash;
        }


        /// <summary>
        /// Computes the hash of the specified string and converts it into hexdecimal.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string SHA512(string str) {
            SHA512 crypt = System.Security.Cryptography.SHA512.Create();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(str), 0, Encoding.UTF8.GetByteCount(str));
            foreach (byte theByte in crypto) {
                hash += theByte.ToString("x2");
            }
            return hash;
        }

#pragma warning disable 0618        
        /// <summary>
        /// Hashes the string with the provided method.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException">The hash method is not supported</exception>
        public static string Hash(string method, string str) {
            switch (method.ToLower()) {
                case "md5":
                    return MD5(str);
                case "sha128":
                    return SHA128(str);
                case "sha256":
                    return SHA256(str);
                case "sha512":
                    return SHA512(str);
                default:
                    throw new NotSupportedException("The hash method is not supported");
            }
        }
#pragma warning restore 0618
        #endregion
    }
}
