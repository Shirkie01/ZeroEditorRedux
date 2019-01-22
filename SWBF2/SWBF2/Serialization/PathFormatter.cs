using System.Collections.Generic;
using System.IO;

namespace SWBF2.Serialization
{
    public class PathFormatter : ITypedFormatter<IList<Path>>
    {
        public IList<Path> Deserialize(Stream serializationStream)
        {
            throw new System.NotImplementedException();
        }

        public void Serialize(Stream serializationStream, IList<Path> obj)
        {
            throw new System.NotImplementedException();
        }
    }
}