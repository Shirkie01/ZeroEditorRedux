using System.Drawing;

namespace SWBF2
{
    public class TerrainBlock
    {
        public short Height { get; set; }
        public short BlendHeight1 { get; set; }
        public short BlendHeight2 { get; set; }
        public Color ForegroundColor { get; set; } = Color.White;
        public Color BackgroundColor { get; set; } = Color.White;
        public byte WaterLayerId;
        public FoliageType FoliageTypes;
        public byte Unknown;
        public bool[] TextureLayerIds = new bool[16];
        public byte[] TextureAlphas = new byte[16];

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (this == obj)
                return true;

            var other = (TerrainBlock)obj;

            if (other == null)
                return false;

            if (Height != other.Height)
                return false;

            if (ForegroundColor != other.ForegroundColor)
                return false;

            if (BackgroundColor != other.BackgroundColor)
                return false;

            if (BlendHeight1 != other.BlendHeight1)
                return false;

            if (BlendHeight2 != other.BlendHeight2)
                return false;

            if (WaterLayerId != other.WaterLayerId)
                return false;

            if (FoliageTypes != other.FoliageTypes)
                return false;

            if (Unknown != other.Unknown)
                return false;

            if (TextureLayerIds.Length != other.TextureLayerIds.Length)
                return false;

            for (int i = 0; i < TextureLayerIds.Length; i++)
            {
                if (TextureLayerIds[i] != other.TextureLayerIds[i])
                    return false;
            }

            if (TextureAlphas.Length != other.TextureAlphas.Length)
                return false;

            for (int i = 0; i < TextureAlphas.Length; i++)
            {
                if (TextureAlphas[i] != other.TextureAlphas[i])
                    return false;
            }

            return true;
        }
    }
}