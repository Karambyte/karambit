using System;

namespace Karambit.Utilities
{
    public class Random
    {
        #region Fields
        private System.Random random = new System.Random();
        #endregion

        #region Methods        
        /// <summary>
        /// Generates a random UInt64.
        /// </summary>
        /// <returns></returns>
        public ulong RandomUInt64() {
            return BitConverter.ToUInt64(RandomBytes(8), 0);
        }

        /// <summary>
        /// Generates a random Int64.
        /// </summary>
        /// <returns></returns>
        public long RandomInt64() {
            return BitConverter.ToInt64(RandomBytes(8), 0);
        }

        /// <summary>
        /// Generates a random UInt32.
        /// </summary>
        /// <returns></returns>
        public uint RandomUInt32() {
            return BitConverter.ToUInt32(RandomBytes(4), 0);
        }

        /// <summary>
        /// Generates a random Int32.
        /// </summary>
        /// <returns></returns>
        public int RandomInt32() {
            return BitConverter.ToInt32(RandomBytes(4), 0);
        }

        /// <summary>
        /// Generates a random UInt16.
        /// </summary>
        /// <returns></returns>
        public ushort RandomUInt16() {
            return BitConverter.ToUInt16(RandomBytes(2), 0);
        }

        /// <summary>
        /// Generates a random Int16.
        /// </summary>
        /// <returns></returns>
        public short RandomInt16() {
            return BitConverter.ToInt16(RandomBytes(2), 0);
        }

        /// <summary>
        /// Generates a random UInt8.
        /// </summary>
        /// <returns></returns>
        public byte RandomUInt8() {
            return RandomBytes(1)[0];
        }

        /// <summary>
        /// Generates a random Int8.
        /// </summary>
        /// <returns></returns>
        public sbyte RandomInt8() {
            return (sbyte)RandomBytes(1)[0];
        }

        /// <summary>
        /// Generates a number of random bytes.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public byte[] RandomBytes(int length) {
            byte[] data = new byte[length];
            random.NextBytes(data);
            return data;
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="Random"/> class.
        /// </summary>
        /// <param name="seed">The seed.</param>
        public Random(int seed) {
            this.random = new System.Random(seed);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Random"/> class.
        /// </summary>
        public Random() {
            this.random = new System.Random();
        }
        #endregion
    }
}
