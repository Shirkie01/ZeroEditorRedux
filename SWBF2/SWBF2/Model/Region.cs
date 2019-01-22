namespace SWBF2
{
    public class Region
    {
        public string Name;
        public int Id;
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Size;
        public Layer layer;

        public Region(string name, int id)
        {
            Name = name;
            Id = id;
        }

        public static Region LoadFromFile(string path)
        {
            return null;
        }
    }
}