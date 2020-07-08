using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using log4net;
using System.IO;
using ZeroEditorRedux.Model;
using SWBF2;

namespace ZeroEditorRedux.Views.Controls.Terrain
{
    /// <summary>
    /// Interaction logic for TerrainLoadTester.xaml
    /// </summary>
    public partial class TerrainVisualizationTester : UserControl
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TerrainVisualizationTester));        

        private int terrainIndex = 0;

        public IList<TerrainDisplayModel> Terrains { get; } = new List<TerrainDisplayModel>();        
        public TerrainDisplayModel SelectedTerrain
        {
            get => Terrains[terrainIndex];
        }

        public TerrainVisualizationTester()
        {
            InitializeComponent();            
            Task.Run(() => LoadTerrainsAsync());
        }

        private void LoadTerrainsAsync()
        {
            LoadTerrains("C:\\BF2_ModTools\\assets");
        }

        private void LoadTerrains(string directory)
        {
            log.Debug("Loading Terrains...");

            var di = new DirectoryInfo(directory);
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
                    SWBF2.Terrain t;
                    using (var fs = new FileStream(fi.FullName, FileMode.Open))
                    {
                        t = TerrainLoader.Load(fs);
                    }

                    progressBar.Dispatcher.Invoke(() =>
                    {
                        var tv3d = TerrainLoader.CreateTerrainV3DFromTerrain(t, fi.Directory.FullName);
                        tv3d.SetName(fi.Name);
                        Terrains.Add(new TerrainDisplayModel(t, tv3d));
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

            var initTerrain = Terrains[terrainIndex];
            TerrainName.Dispatcher.Invoke(() =>
            {
                TerrainName.Text = initTerrain.Graphics.GetName();
            });
            
            helixViewport3D.Dispatcher.Invoke(() => helixViewport3D.Children.Add(initTerrain.Graphics));
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            helixViewport3D.Children.Remove(Terrains[terrainIndex].Graphics);
            if (++terrainIndex > Terrains.Count - 1)
            {
                terrainIndex = 0;
            }
            SelectTerrain(terrainIndex);
            var terrainGraphics = SelectedTerrain.Graphics;
            TerrainName.Text = SelectedTerrain.Graphics.GetName();
            helixViewport3D.Children.Add(terrainGraphics);
        }

        private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            helixViewport3D.Children.Remove(Terrains[terrainIndex].Graphics);
            if (--terrainIndex < 0)
            {
                terrainIndex = Terrains.Count - 1;
            }
            SelectTerrain(terrainIndex);
            var terrain = Terrains[terrainIndex];
            TerrainName.Text = terrain.Graphics.GetName();
            helixViewport3D.Children.Add(terrain.Graphics);
        }

        public void SelectTerrain(int terrainIndex)
        {
            this.terrainIndex = terrainIndex;
        }
    }
}
