using HelixToolkit.Wpf;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using TgaLib;

namespace ZeroEditorRedux
{
    internal class DiffuseTexture : TerrainTexture
    {
        public DiffuseTexture(string source)
        {
            using (var reader = new BinaryReader(new FileStream(source, FileMode.Open)))
            {
                var brush = new ImageBrush(new TgaImage(reader).GetBitmap());
                Material = MaterialHelper.CreateMaterial(brush, 0, byte.MaxValue, true);
            }
        }

        public override void Calculate(TerrainModel model, MeshGeometry3D mesh)
        {
            var texcoords = new PointCollection();
            foreach (var p in mesh.Positions)
            {
                double x = p.X + model.Offset.X;
                double y = p.Y + model.Offset.Y;
                texcoords.Add(new System.Windows.Point(x, y));
            }

            this.TextureCoordinates = texcoords;
        }
    }
}