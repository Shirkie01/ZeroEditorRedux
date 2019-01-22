using System.Collections.Generic;

namespace SWBF2
{
    public class Terrain
    {
        public TerrainHeader Header { get; set; } = new TerrainHeader();

        public byte[] DecalBytes;
        public TerrainBlock[,] Blocks { get; set; } = new TerrainBlock[1024, 1024];

        public byte[] UnknownBytes1 = new byte[1024 * 1024 / 8];
        public byte[] UnknownBytes2 = new byte[1024 * 1024 / 4];

        public int EndOfFileLength = 4;
        public int NumberOfItems = 0;
        public List<float[]> EndOfFileItems = new List<float[]>();

        public Terrain()
        {
        }
    }
}