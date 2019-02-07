using SWBF2;
using System.Collections.Generic;

namespace ZeroEditorRedux.ViewModels
{
    internal class HintNodeViewModel : ViewModelBase
    {
        private HintNode _selectedHintNode;

        public HintNode SelectedHintNode
        {
            get => _selectedHintNode;
            set { SetProperty(ref _selectedHintNode, value); }
        }

        public IList<HintNode> HintNodes { get; } = new List<HintNode>();
    }
}