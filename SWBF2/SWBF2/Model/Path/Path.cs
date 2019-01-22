using System.Collections.Generic;
using System.IO;

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

        public static Path LoadFromFile(string path)
        {
            string text = string.Empty;
            using (StreamReader sr = new StreamReader(path))
            {
                text = sr.ReadToEnd();
            }

            return null;
        }
    }
}