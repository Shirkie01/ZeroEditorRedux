namespace SWBF2
{
    public class TextureLayer
    {
        public string DiffuseTexture { get; set; }
        public string DetailTexture { get; set; }
        public byte MappingType { get; set; }

        // The scale of the texture
        public float TileRange { get; set; }

        public float Rotation { get; set; }

        public override string ToString()
        {
            return "DiffuseTexture=" + DiffuseTexture + ", DetailTexture=" + DetailTexture + ", MappingType=" + MappingType + ", TileRange=" + TileRange + ", Rotation=" + Rotation;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;

            var other = (TextureLayer)obj;
            if (other == null)
                return false;

            if (!Equals(DiffuseTexture, other.DiffuseTexture))
                return false;

            if (!Equals(DetailTexture, other.DetailTexture))
                return false;

            if (!Equals(MappingType, other.MappingType))
                return false;

            if (!Equals(TileRange, other.TileRange))
                return false;

            if (!Equals(Rotation, other.Rotation))
                return false;

            return true;
        }
    }
}