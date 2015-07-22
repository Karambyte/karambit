using Karambit.IO;
using System;
using System.IO;

namespace Karambit.Serialization
{
    public class ObjectSerializer : Serializer
    {
        #region Enums
        #endregion

        #region Methods
        public override T Deserialize<T>(Stream stream) {
            throw new NotImplementedException();
        }

        public override void Serialize(Stream stream, object obj) {
            throw new NotImplementedException();
        }

        public override bool CanDeserialize {
            get {
                return true;
            }
        }

        public override bool CanSerialize {
            get {
                return true;
            }
        }
        #endregion
    }
}
