using SWBF2;
using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace ZeroEditorRedux
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ICommand NewWorldCommand { get; }
        public ICommand OpenWorldCommand { get; }
        public ICommand SaveWorldCommand { get; }
        public ICommand SetModToolsDirectoryCommand { get; }
        public ICommand SetSWBF2DirectoryCommand { get; }
        public ICommand ExitCommand { get; }

        public TerrainViewModel TerrainViewModel { get; } = new TerrainViewModel();

        private FileInfo _openFileInfo;

        public string AppTitle => _openFileInfo?.Name;

        private Layer _activeLayer;

        public Layer ActiveLayer
        {
            get => _activeLayer;
            set => SetProperty(ref _activeLayer, value);
        }

        private World _world;

        public World World
        {
            get => _world;
            private set => SetProperty(ref _world, value);
        }

        private GeometryModel3D _terrainModel;

        public GeometryModel3D TerrainModel
        {
            get => _terrainModel;
            set => SetProperty(ref _terrainModel, value);
        }

        private bool _gridLinesVisible = true;

        public bool GridLinesVisible
        {
            get => _gridLinesVisible;
            set => SetProperty(ref _gridLinesVisible, value);
        }

        private string _swbf2ModToolsDirectory = Properties.Settings.Default.SWBF2_MODTOOLS;

        public string SWBF2ModToolsDirectory
        {
            get => _swbf2ModToolsDirectory;
            set
            {
                Properties.Settings.Default.SWBF2_MODTOOLS = value;
                SetProperty(ref _swbf2ModToolsDirectory, value);
            }
        }

        private string _swbf2GameDataDirectory = Properties.Settings.Default.SWBF2_DIRECTORY;

        public string SWBF2GameDataDirectory
        {
            get => _swbf2ModToolsDirectory;
            set
            {
                Properties.Settings.Default.SWBF2_DIRECTORY = value;
                SetProperty(ref _swbf2GameDataDirectory, value);
            }
        }

        private string _swbf2DataDirectory;

        public MainWindowViewModel()
        {
            OpenWorldCommand = new RelayCommand<object>(OpenWorld);
            SaveWorldCommand = new RelayCommand<object>(SaveWorld);
            SetModToolsDirectoryCommand = new RelayCommand<object>(((ignore) => BrowseForDirectory((dir) => SWBF2ModToolsDirectory = dir)));
            SetSWBF2DirectoryCommand = new RelayCommand<object>(((ignore) => BrowseForDirectory((dir) => SWBF2GameDataDirectory = dir)));
            ExitCommand = new RelayCommand<object>(new Action<object>(Exit));
        }

        private void OpenWorld(object parameter)
        {
            using (var openDlg = new OpenFileDialog()
            {
                Filter = "WLD Files|*.wld"
            })
            {
                openDlg.InitialDirectory = _openFileInfo?.DirectoryName;
                if (openDlg.ShowDialog() == DialogResult.OK)
                {
                    ActiveLayer = World.Layers[0];
                    _openFileInfo = new FileInfo(openDlg.FileName);

                    var dir = _openFileInfo.Directory;
                    while (!dir.Name.StartsWith("data_"))
                    {
                        dir = dir.Parent;
                    }
                    _swbf2DataDirectory = dir.Name;

                    TerrainViewModel.SelectedTerrain = World.Terrain;

                    OnPropertyChanged();
                }
            }
        }

        public void SaveWorld(object parameter)
        {
        }

        public void BrowseForDirectory(Action<string> setDirectoryAction)
        {
            using (var fd = new FolderBrowserDialog())
            {
                var result = fd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    setDirectoryAction(fd.SelectedPath);
                }
            }
        }

        public void Exit(object parameter)
        {
            Application.Exit();
        }
    }
}