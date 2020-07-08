using HelixToolkit.Wpf;
using log4net;
using SWBF2;
using SWBF2.Serialization;
using System;
using System.IO;
using System.Windows.Media.Media3D;

namespace ZeroEditorRedux.Model
{
    public static class TerrainLoader
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TerrainLoader));

        private static TerrainFormatter formatter = new TerrainFormatter();

        public static Terrain Load(Stream stream)
        {
            return formatter.Deserialize(stream);
        }

        private static void LoadTerrainUsingExtents(Terrain terrain, TerrainModel terrainModel)
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

        private static void LoadTerrainUsingGridSize(Terrain terrain, TerrainModel terrainModel)
        {
            var gridSize = terrain.Header.GridSize;
            terrainModel.Width = gridSize;
            terrainModel.Height = gridSize;

            terrainModel.Left = -gridSize * terrain.Header.GridScale / 2;
            terrainModel.Right = gridSize * terrain.Header.GridScale / 2;
            terrainModel.Top = -gridSize * terrain.Header.GridScale / 2;
            terrainModel.Bottom = gridSize * terrain.Header.GridScale / 2;

            terrainModel.Data = new double[gridSize * gridSize];

            int index = 0;
            for (int x = 0; x < terrain.Header.GridSize; x++)
            {
                for (int y = 0; y < terrain.Header.GridSize; y++)
                {
                    var h = terrain.Blocks[x, y].Height * terrain.Header.MapHeightMultiplier;
                    terrainModel.Data[index++] = h;
                }
            }              
            
        }

        private static void LoadTerrainTexture(Terrain terrain, TerrainModel terrainModel, string path)
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

        public static TerrainVisual3D CreateTerrainV3DFromTerrain(Terrain terrain, string path)
        {
            if (terrain == null)
            {
                throw new ArgumentNullException(nameof(terrain));
            }

            var terrainV3D = new TerrainVisual3D();
            var terrainModel = new TerrainModel();

            // Do Terrain stuff
            LoadTerrainUsingGridSize(terrain, terrainModel);
            //LoadTerrainUsingExtents(terrain, terrainModel);

            LoadTerrainTexture(terrain, terrainModel, path);

            ((ModelVisual3D)(terrainV3D.Children[0])).Content = terrainModel.CreateModel(1);
            return terrainV3D;
        }
    }
}
