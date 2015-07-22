using System;

namespace Karambit.Serialization
{
    public class EmptySerializer : Serializer
    {
        #region Methods        
        /// <summary>
        /// Deserializes the object from the specified stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override T Deserialize<T>(System.IO.Stream stream) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Serializes the object to the specified stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="obj">The object.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Serialize<T>(System.IO.Stream stream, T obj) {
            throw new NotImplementedException();
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptySerializer"/> class.
        /// </summary>
        public EmptySerializer() {
        }
        #endregion
    }
}
