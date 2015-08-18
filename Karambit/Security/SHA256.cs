using System;
using System.Security.Cryptography;
using System.Text;

namespace Karambit.Security
{
    public static class Sha256
    {
        #region Methods                
        /// <summary>
        /// Computes the hash of the specified string and converts it into hexdecimal.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string Compute(string str) {
            SHA256 crypt = SHA256.Create();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(str), 0, Encoding.ASCII.GetByteCount(str));
            foreach (byte theByte in crypto) {
                hash += theByte.ToString("x2");
            }
            return hash;
        }
        #endregion
    }
}
