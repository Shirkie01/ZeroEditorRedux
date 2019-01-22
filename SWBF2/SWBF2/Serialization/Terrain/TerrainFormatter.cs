using System.IO;

namespace SWBF2.Serialization
{
    /// <summary>
    /// Reads or writes a SWBF/SWBF2 TER file.
    /// </summary>
    public class TerrainFormatter : ITypedFormatter<Terrain>
    {
        /// <summary>
        /// Reads the TER file and returns a terrain object
        /// </summary>
        /// <param name="serializationStream">The stream to read the TER file from</param>
        /// <returns>A terrain object</returns>
        public Terrain Deserialize(Stream serializationStream)
        {
            Terrain terrain = null;
            using (var reader = new TerrainReader(serializationStream))
            {
                terrain = reader.ReadTerrain();
            }

            return terrain;
        }

        /// <summary>
        /// Writes a terrain object to the provided stream
        /// </summary>
        /// <param name="serializationStream">The stream to write the TER file to</param>
        /// <param name="graph">The terrain object</param>
        public void Serialize(Stream serializationStream, Terrain terrain)
        {
            using (var writer = new TerrainWriter(serializationStream))
            {
                writer.Write(terrain);
            }
        }
    }
}