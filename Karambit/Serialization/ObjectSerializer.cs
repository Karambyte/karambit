using Karambit.IO;
using System;

namespace Karambit.Serialization
{
    public class ObjectSerializer : Serializer
    {
        #region Enums
        enum 
        #endregion

        #region Methods
        public override T Deserialize<T>(System.IO.Stream stream) {
            // read type
            byte typeCode = (byte)stream.ReadByte();

            TypeCode.

            ObjectReader reader = new ObjectReader();
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
