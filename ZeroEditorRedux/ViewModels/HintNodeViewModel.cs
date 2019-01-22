using SWBF2;
using System.Collections.Generic;

namespace ZeroEditorRedux.ViewModels
{
    internal class HintNodeViewModel : ViewModelBase
    {
        private Hint _selectedHintNode;

        public Hint SelectedHintNode
        {
            get => _selectedHintNode;
            set { SetProperty(ref _selectedHintNode, value); }
        }

        private IList<Hint> HintNodes;
    }
}