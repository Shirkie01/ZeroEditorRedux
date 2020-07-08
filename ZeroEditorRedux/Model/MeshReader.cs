using HelixToolkit.Wpf;
using SWBF2.Model.Mesh;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace ZeroEditorRedux.Model
{
    public class MeshReader : ModelReader
    {
        public override Model3DGroup Read(string path)
        {
            Mesh mesh = Mesh.Load(path);
            


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
