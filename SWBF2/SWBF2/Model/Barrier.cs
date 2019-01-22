using System.Collections.Generic;

namespace SWBF2
{
    public class Barrier
    {
        public string Name { get; set; }
        public IList<Vector3> Corners { get; } = new List<Vector3>();
        public int Flag { get; set; }

        public Barrier(string name)
        {
            Name = name;
        }
    }
}