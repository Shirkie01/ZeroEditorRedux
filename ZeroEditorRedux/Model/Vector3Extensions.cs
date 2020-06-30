using SWBF2;
using System.Windows.Media.Media3D;

namespace ZeroEditorRedux.Model
{
    public static class Vector3Extensions
    {
        public static Point3D ToPoint3D(this Vector3 vector)
        {
            return new Point3D(vector.X, vector.Y, vector.Z);
        }
    }
}
