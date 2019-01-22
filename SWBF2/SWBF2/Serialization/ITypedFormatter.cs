using System.IO;

namespace SWBF2.Serialization
{
    /// <summary>
    /// Provides functionality for serializing objects
    /// </summary>
    /// <typeparam name="T">The object to serialize or deserialize</typeparam>
    public interface ITypedFormatter<T>
    {
        T Deserialize(Stream serializationStream);

        void Serialize(Stream serializationStream, T obj);
    }
}