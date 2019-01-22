namespace SWBF2
{
    public struct TerrainExtents
    {
        public short MinX { get; }
        public short MinY { get; }

        public short MaxX { get; }
        public short MaxY { get; }

        public TerrainExtents(short minX, short minY, short maxX, short maxY)
        {
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
        }

        public override string ToString()
        {
            return "(" + MinX + ", " + MinY + ") (" + MaxX + ", " + MaxY + ")";
        }
    }
}