using System.Collections.Generic;

namespace SWBF2
{
    public class Group
    {
        public string Name;
        public IList<Layer> Layers { get; } = new List<Layer>();

        public Group(string name, int int1, int int2)
        {
            Name = name;
        }
    }
}