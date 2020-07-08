using log4net;
using System.Windows;
using ZeroEditorRedux.Model;
using ZeroEditorRedux.ViewModels;

namespace ZeroEditorRedux
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MainWindow));        

        public MainWindow()
        {
            InitializeComponent();
            var mainWindowViewModel = new MainWindowViewModel();

            DataContext = mainWindowViewModel;

            var reader = new MeshReader();
            TestModel.Content = reader.Read(@"C:\BF2_ModTools\assets\worlds\KAM\msh\imp_fly_trooptrans.msh");
        }
    }
}