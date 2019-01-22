using System.Collections.Generic;

namespace SWBF2
{
    public class GameMode
    {
        public string Name;
        public IList<Layer> Layers = new List<Layer>();

        public GameMode(string name)
        {
            Name = name;
        }
    }
}