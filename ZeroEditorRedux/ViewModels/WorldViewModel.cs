using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ZeroEditorRedux.ViewModels
{
    internal class WorldViewModel : ViewModelBase
    {
        public ICommand Open => new RelayCommand<string>(OpenWorld);


        private void OpenWorld(string filePath)
        { 
            
        }        
    }
}
