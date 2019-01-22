namespace SWBF2
{
    public class Layer
    {
        public string Name;
        public string Description;
        public Camera Camera;
        public string LightName;
        public WorldExtents WorldExtents;
        public int Version;
        public int SaveType;

        public Layer(string name, int int1)
        {
            Name = name;
        }
    }
}