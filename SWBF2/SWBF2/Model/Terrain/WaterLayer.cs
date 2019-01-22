using System.Drawing;

namespace SWBF2
{
    public class WaterLayer
    {
        public float Height1 { get; set; }
        public float Height2 { get; set; }
        public float Unknown1 { get; set; } // Possibly damage
        public float Unknown2 { get; set; }
        public UVAnimation UVAnimation { get; set; } = new UVAnimation();
        public Color Color { get; set; }
        public string TextureName { get; set; }
    }

    public class UVAnimation
    {
        public PointF Velocity { get; set; } = new PointF();
        public PointF Repeat { get; set; } = new PointF();
    }
}