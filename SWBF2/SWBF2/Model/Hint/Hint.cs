namespace SWBF2
{
    public class Hint
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public string Name;
        public HintType Type;

        public PrimaryStance PrimaryStance;
        public SecondaryStance SecondaryStance;

        public HintMode HintMode;

        public string CommandPost;

        public Hint(string name, HintType type)
        {
            Name = name;
            Type = type;
        }

        public double Radius { get; set; }
    }
}