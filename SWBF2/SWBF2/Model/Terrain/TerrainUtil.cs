using System;

namespace SWBF2
{
    public static class TerrainUtil
    {
        public static void ForEveryPoint(int gridSize, Action<int, int> action, int step = 1)
        {
            for (int x = 0; x < gridSize; x += step)
            {
                for (int y = 0; y < gridSize; y += step)
                {
                    action(x, y);
                }
            }
        }
    }
}