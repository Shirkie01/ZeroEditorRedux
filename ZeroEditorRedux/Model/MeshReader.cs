using HelixToolkit.Wpf;
using System;
using System.IO;
using System.Windows.Media.Media3D;

namespace ZeroEditorRedux.Model
{
    public class MeshReader : ModelReader
    {
        public override Model3DGroup Read(string path)
        {
            return new Model3DGroup()
            { 
                    
            };
        }

        public override Model3DGroup Read(Stream s)
        {
            throw new NotImplementedException();
        }
    }
}
