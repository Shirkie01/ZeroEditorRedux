namespace SWBF2
{
    public class TerrainHeader
    {
        public string Name { get; set; }
        public TerrainVersion Version { get; set; }
        public TerrainExtents Extents { get; set; }
        public int Unknown1 { get; set; }
        public float MapHeightMultiplier { get; set; }
        public float GridScale { get; set; } = 8;
        public int Unknown2 { get; set; }
        public int GridSize { get; set; }
        public InGameOptions InGameOptions { get; set; }

        public byte SWBF2Byte { get; set; }
        public TextureLayer[] TextureLayers { get; set; } = new TextureLayer[16];
        public WaterLayer[] WaterLayers { get; set; } = new WaterLayer[16];
        public string[] DecalTextureNames { get; set; } = new string[16];
        public int DecalLength { get; set; }

        // Possibly related to decals
        public byte[] UnknownBytes = new byte[8];

        public TerrainHeader(int gridSize = 1024)
        {
            GridSize = gridSize;

            for (int i = 0; i < 16; i++)
            {
                TextureLayers[i] = new TextureLayer();
                WaterLayers[i] = new WaterLayer();
            }
        }
    }
}