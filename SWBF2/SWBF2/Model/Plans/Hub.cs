using System.Collections.Generic;

namespace SWBF2
{
    public class Hub
    {
        public string Name { get; set; }

        public Vector3 Position { get; set; }
        public double Radius { get; set; }

        public IList<BranchWeight> BranchWeights { get; set; }

        public Hub(string name)
        {
            Name = name;
        }
    }
}