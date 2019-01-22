using System.Collections.Generic;

namespace SWBF2
{
    public class Path
    {
        public string Name;
        public int Data;
        public PathType PathType;
        public PathSpeedType PathSpeedType;
        public float PathTime;
        public int OffsetPath;
        public SplineType SplineType;

        public IList<Node> Nodes = new List<Node>();

        public Path(string name)
        {
            Name = name;
        }
    }
}