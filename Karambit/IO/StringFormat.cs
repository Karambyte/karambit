using System;

namespace Karambit.IO
{
    public enum StringFormat
    {
        /// <summary>
        /// .NET style 7-bit encoded length prefix
        /// </summary>
        LengthEncoded,
        /// <summary>
        /// Null terminated
        /// </summary>
        NullTerminated,

        /// <summary>
        /// Fixed length, provide length as third parameter
        /// </summary>
        FixedSize,

        /// <summary>
        /// 8-bit length prefix
        /// </summary>
        LengthTiny,

        /// <summary>
        /// 16-bit length prefix
        /// </summary>
        LengthMedium,

        /// <summary>
        /// 32-bit length prefix
        /// </summary>
        LengthLarge
    }
}
