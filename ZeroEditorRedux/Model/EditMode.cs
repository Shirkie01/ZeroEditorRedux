namespace ZeroEditorRedux.Model
{
    internal enum EditMode
    {
        HEIGHT, // Edit height properties of the vertex map terrain
        COLOR, // Edit color of the vertex map
        TEXTURE, // Edit textures applied to the vertex map terrain
        WATER, // Add water and edit water texture properties
        FOLIAGE, // Paint and erase foliage on terrain
        OBJECT, // Add and edit object location and properties
        PATH, // Add and edit unit spawn paths
        REGIONS, // Add and edit region properties
        PORTAL,
        HINTNODE, // Add and edit AI hint nodes
        BARRIER, // Add and edit AI barriers
        PLANNING, // Add and edit AI path planning connectivity graph
        BOUNDARY, // Add and edit map boundaries
        LIGHT, // Add and edit lighting
    }
}