using HelixToolkit.Wpf;
using SWBF2;
using System.Collections.Generic;
using System.Windows.Media.Media3D;
using ZeroEditorRedux.Controls;

namespace ZeroEditorRedux.ViewModels
{
    internal class HintNodeViewModel : ViewModelBase
    {
        public HintNodeVisual3D Graphics { get; }
        public HintNode HintNode { get; }

        public HintNodeViewModel(HintNode hintNode)
        {
            HintNode = hintNode;            
        }     
    }
}