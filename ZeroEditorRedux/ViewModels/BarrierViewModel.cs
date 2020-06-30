using HelixToolkit.Wpf;
using SWBF2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroEditorRedux.Model;

namespace ZeroEditorRedux.ViewModels
{
    internal class BarrierViewModel : ViewModelBase
    {
        public BoxVisual3D Graphics { get; }
        public Barrier Barrier { get; }

        public BarrierViewModel(Barrier barrier)
        {
            Barrier = barrier;
            Graphics = new BoxVisual3D()
            {
                Center = barrier.Corners[0].ToPoint3D()
            };
        }
    }
}
