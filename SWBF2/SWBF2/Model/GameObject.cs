using System.Collections.Generic;

namespace SWBF2
{
    public class GameObject
    {
        public string Name;
        public string GeometryName;
        public float GeometryScale;

        public IDictionary<string, string> Properties;

        public IDictionary<string, string> InstanceProperties;

        public GameObject(string name, string odfFileName, int sequenceNumber)
        {
            Name = name;
        }
    }
}