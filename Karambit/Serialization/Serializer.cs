using System;
using System.IO;

namespace Karambit.Serialization
{
    public abstract class Serializer
    {
        #region Properties        
        /// <summary>
        /// Gets a value indicating whether this serializer can serialize.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can serialize; otherwise, <c>false</c>.
        /// </value>
        public virtual bool CanSerialize {
            get {
                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this serializer can deserialize.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can deserialize; otherwise, <c>false</c>.
        /// </value>
        public virtual bool CanDeserialize {
            get {
                return true;
            }
        }
        #endregion

        #region Methods        
        /// <summary>
        /// Deserializes the object from the specified stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public abstract T Deserialize<T>(Stream stream);

        /// <summary>
        /// Serializes the object to the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="obj">The object.</param>
        public abstract void Serialize(Stream stream, object obj);
        
        /// <summary>
        /// Serializes the specified object to binary data.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public byte[] Serialize(object obj) {
            using (MemoryStream ms = new MemoryStream()) {
                Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Deserializes the object from the specified binary data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public T Deserialize<T>(byte[] data) {
            using (MemoryStream ms = new MemoryStream(data)) {
                return Deserialize<T>(ms);
            }
        }
        #endregion
    }
}
