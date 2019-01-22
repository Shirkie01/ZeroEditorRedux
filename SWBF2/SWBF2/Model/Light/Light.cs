using System.Drawing;

namespace SWBF2
{
    public class Light
    {
        public string Name;
        public int Id { get; }
        public Quaternion Rotation;
        public Vector3 Position;
        public LightType Type;
        public bool CastShadow;
        public bool Static;
        public string Texture;
        public PointF TileUV;
        public PointF OffsetUV;
        public bool Wrap;
        public Color Color;
        public bool CastSpecular;
        public bool PS2Overbright;
        public Region Region;
        public PS2BlendMode ps2BlendMode;

        public float Range;

        public Light(string name, int id)
        {
            Name = name;
            Id = id;
        }
    }
}