using HelixToolkit.Wpf;
using log4net;
using SWBF2;
using SWBF2.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace ZeroEditorRedux
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MainWindow));

        private TerrainFormatter formatter = new TerrainFormatter();

        private IList<TerrainVisual3D> terrains = new List<TerrainVisual3D>();

        private int terrainIndex = 0;

        public MainWindow()
        {
            InitializeComponent();
            var mainWindowViewModel = new MainWindowViewModel();

            DataContext = mainWindowViewModel;
            progressBar.IsIndeterminate = true;
            Task.Run(() => LoadTerrainsAsync(mainWindowViewModel));
        }

        private void LoadTerrainsAsync(MainWindowViewModel mainWindowViewModel)
        {
            var swbf2Terrains = LoadTerrains();
            ((List<Terrain>)mainWindowViewModel.TerrainViewModel.Terrains).AddRange(swbf2Terrains);
        }

        private IList<Terrain> LoadTerrains()
        {
            log.Debug("Loading Terrains...");
            IList<Terrain> swbf2Terrains = new List<Terrain>();

            var di = new DirectoryInfo("C:\\BF2_ModTools\\assets");
            var fileInfos = di.GetFiles("*.TER", SearchOption.AllDirectories);

            progressBar.Dispatcher.Invoke(() =>
            {
                progressBar.Maximum = fileInfos.Length;
                progressBar.IsIndeterminate = false;
            });            

            foreach (var fi in fileInfos)
            {
                log.Debug($"Loading {fi.Name}...");
                progressText.Dispatcher.Invoke(() =>
                {
                    progressText.Text = fi.FullName;
                });


                try
                {
                    var t = formatter.Deserialize(new FileStream(fi.FullName, FileMode.Open));
                    swbf2Terrains.Add(t);
                    progressBar.Dispatcher.Invoke(() =>
                    {
                        var tv3d = CreateTerrainV3DFromTerrain(t, fi.Directory.FullName);
                        tv3d.SetName(fi.Name);
                        terrains.Add(tv3d);
                    });
                }
                catch (NotImplementedException)
                {
                    log.Warn($"Terrain '{fi.Name}' has a feature which is not implemented. Continuing...");
                }


                progressBar.Dispatcher.Invoke(() =>
                {
                    progressBar.Value++;
                });
            }

            // hide the progress bar
            progressBar.Dispatcher.Invoke(() =>
            {
                progressBar.Visibility = Visibility.Collapsed;
                progressText.Visibility = Visibility.Collapsed;
            });

            var initTerrain = terrains[terrainIndex];
            TerrainName.Dispatcher.Invoke(() =>
            {
                TerrainName.Text = initTerrain.GetName();
            });
            htkViewport3d.Dispatcher.Invoke(() => htkViewport3d.Children.Add(initTerrain));
            return swbf2Terrains;
        }

        private TerrainVisual3D CreateTerrainV3DFromTerrain(Terrain terrain, string path)
        {
            var terrainV3D = new TerrainVisual3D();
            var terrainModel = new TerrainModel();

            // Do Terrain stuff
            LoadTerrainUsingGridSize(terrain, terrainModel);
            //LoadTerrainUsingExtents(terrain, terrainModel);

            LoadTerrainTexture(terrain, terrainModel, path);

            ((ModelVisual3D)(terrainV3D.Children[0])).Content = terrainModel.CreateModel(1);
            return terrainV3D;
        }

        private void LoadTerrainUsingGridSize(Terrain terrain, TerrainModel terrainModel)
        {
            var gridSize = terrain.Header.GridSize;
            terrainModel.Width = gridSize;
            terrainModel.Height = gridSize;

            terrainModel.Left = -gridSize / 2;
            terrainModel.Right = gridSize / 2;
            terrainModel.Top = -gridSize / 2;
            terrainModel.Bottom = gridSize / 2;

            terrainModel.Data = new double[terrain.Header.GridSize * terrain.Header.GridSize];

            int index = 0;
            for (int x = 0; x < terrain.Header.GridSize; x++)
            {
                for (int y = 0; y < terrain.Header.GridSize; y++)
                {
                    var h = terrain.Blocks[x, y].Height * terrain.Header.MapHeightMultiplier / terrain.Header.GridScale;
                    terrainModel.Data[index++] = h;
                }
            }
        }

        private void LoadTerrainUsingExtents(Terrain terrain, TerrainModel terrainModel)
        {
            var extents = terrain.Header.Extents;
            terrainModel.Width = extents.MaxX - extents.MinX;
            terrainModel.Height = extents.MaxY - extents.MinY;

            terrainModel.Left = terrain.Header.Extents.MinX;
            terrainModel.Right = terrain.Header.Extents.MaxX;
            terrainModel.Top = terrain.Header.Extents.MinY;
            terrainModel.Bottom = terrain.Header.Extents.MaxY;

            terrainModel.Data = new double[extents.MaxX * extents.MaxY * 4];

            int index = 0;
            for (int x = extents.MinX; x < extents.MaxX; x++)
            {
                for (int y = extents.MinY; y < extents.MaxY; y++)
                {
                    var h = terrain.Blocks[x - extents.MinX, y - extents.MinY].Height * terrain.Header.MapHeightMultiplier / terrain.Header.GridScale;
                    terrainModel.Data[index++] = h;
                }
            }
        }

        private void LoadTerrainTexture(Terrain terrain, TerrainModel terrainModel, string path)
        {
            string textureName = terrain.Header.TextureLayers[0].DiffuseTexture;

            try
            {
                var tex = new DiffuseTexture(path + "\\" + textureName);
                terrainModel.Texture = tex;
            }
            catch (FileNotFoundException e)
            {
                log.Debug($"Could not find file '{textureName}', attempting to search all directories", e);

                // File doesn't exist in world directory. Search above.
                var fileInfos = new DirectoryInfo(path).Parent.GetFiles("*.tga", SearchOption.AllDirectories);

                foreach (var fi in fileInfos)
                {
                    if (fi.Name == textureName)
                    {
                        terrainModel.Texture = new DiffuseTexture(fi.FullName);
                        break;
                    }
                }
            }
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            htkViewport3d.Children.Remove(terrains[terrainIndex]);
            if (++terrainIndex > terrains.Count - 1)
            {
                terrainIndex = 0;
            }
            ((MainWindowViewModel)DataContext).TerrainViewModel.SelectTerrain(terrainIndex);
            var terrain = terrains[terrainIndex];
            TerrainName.Text = terrain.GetName();
            htkViewport3d.Children.Add(terrain);
        }

        private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            htkViewport3d.Children.Remove(terrains[terrainIndex]);
            if (--terrainIndex < 0)
            {
                terrainIndex = terrains.Count - 1;
            }
            ((MainWindowViewModel)DataContext).TerrainViewModel.SelectTerrain(terrainIndex);
            var terrain = terrains[terrainIndex];
            TerrainName.Text = terrain.GetName();
            htkViewport3d.Children.Add(terrain);
        }
    }
}