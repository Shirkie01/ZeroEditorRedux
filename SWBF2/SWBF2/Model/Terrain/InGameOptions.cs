using System;

namespace SWBF2
{
    [Flags]
    public enum InGameOptions
    {
        None = 0,
        Terrain = 1,
        Water = 2,
        Foliage = 4
    }
}