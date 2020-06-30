using HelixToolkit.Wpf;
using SWBF2;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace ZeroEditorRedux.ViewModels
{
    internal class HintNodeViewModel : ViewModelBase
    {
        public ArrowVisual3D Graphics { get; }
        public HintNode HintNode { get; }

        public HintNodeViewModel(HintNode hintNode)
        {
            HintNode = hintNode;
            Graphics = new ArrowVisual3D()
            {
                Point1 = new Point3D(hintNode.Position.X, hintNode.Position.Y, hintNode.Position.Z),
                Point2 = new Point3D(hintNode.Position.X, hintNode.Position.Y, hintNode.Position.Z + 1)                
            };
        }     
    }
}