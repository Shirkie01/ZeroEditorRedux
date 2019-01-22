﻿using log4net;
using SWBF2;
using System.Collections.Generic;

namespace ZeroEditorRedux
{
    internal class TerrainViewModel : ViewModelBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TerrainViewModel));

        private Terrain _terrain;

        public Terrain SelectedTerrain
        {
            get => _terrain;
            set { SetProperty(ref _terrain, value); }
        }

        public IList<Terrain> Terrains { get; } = new List<Terrain>();

        public void SelectTerrain(int index)
        {
            SelectedTerrain = Terrains[index];
        }
    }
}