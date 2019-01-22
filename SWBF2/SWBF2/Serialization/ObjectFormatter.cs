using System.Collections.Generic;
using System.IO;

namespace SWBF2.Serialization
{
    public class ObjectFormatter : ITypedFormatter<IList<GameObject>>
    {
        public IList<GameObject> Deserialize(Stream serializationStream)
        {
            throw new System.NotImplementedException();
        }

        public void Serialize(Stream serializationStream, IList<GameObject> obj)
        {
            throw new System.NotImplementedException();
        }
    }
}