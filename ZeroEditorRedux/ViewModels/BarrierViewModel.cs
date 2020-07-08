using HelixToolkit.Wpf;
using SWBF2;
using System.Windows.Media.Media3D;

namespace ZeroEditorRedux.ViewModels
{
    internal class BarrierViewModel : ViewModelBase
    {
        public MeshGeometryVisual3D Graphics { get; }
        public Barrier Barrier { get; }

        public BarrierViewModel(Barrier barrier)
        {
            Barrier = barrier;
            Graphics = new MeshGeometryVisual3D()
            {
                
            };
        }
    }
}
