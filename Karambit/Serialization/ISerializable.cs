using Karambit.IO;
using System;

namespace Karambit.Serialization
{
    public interface ISerializable
    {
        void Serialize(ObjectWriter writer);
        void Deserialize(ObjectReader reader);
    }
}
