using Karambit.IO;
using System;

namespace Karambit.Serialization
{
    public class ObjectSerializer : Serializer
    {
        #region Enums
        #endregion

        #region Methods
        public override T Deserialize<T>(System.IO.Stream stream) {
            throw new NotImplementedException();
        }

        public override void Serialize<T>(System.IO.Stream stream, T obj) {
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
