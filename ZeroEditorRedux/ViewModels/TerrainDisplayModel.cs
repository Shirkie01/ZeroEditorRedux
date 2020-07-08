using HelixToolkit.Wpf;
using SWBF2;
using System.Collections.Generic;
using ZeroEditorRedux.Model;

namespace ZeroEditorRedux
{
    public class TerrainDisplayModel
    {
        public Terrain Terrain { get; }
        public TerrainVisual3D Graphics { get; }

        public TerrainDisplayModel(Terrain terrain, TerrainVisual3D graphics)
        {
            Terrain = terrain;
            Graphics = graphics;
        }       
    }
}