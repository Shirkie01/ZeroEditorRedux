namespace ZeroEditorRedux.Model
{
    internal enum EditMode
    {
        None, // No edit mode selected
        Height, // Edit height properties of the vertex map terrain
        Color, // Edit color of the vertex map
        Texture, // Edit textures applied to the vertex map terrain
        Water, // Add water and edit water texture properties
        Foliage, // Paint and erase foliage on terrain
        Object, // Add and edit object location and properties
        Path, // Add and edit unit spawn paths
        Region, // Add and edit region properties
        Portal,
        HintNode, // Add and edit AI hint nodes
        Barrier, // Add and edit AI barriers
        Planning, // Add and edit AI path planning connectivity graph
        Boundary, // Add and edit map boundaries
        Light, // Add and edit lighting
    }
}