namespace SWBF2
{
    public class Region
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Size { get; set; }
        public int LayerId { get; set; } = -1;
        public string RegionType { get; set; }
        public bool NextIsGrouped { get; set; }

        public Region(string regionType, int id)
        {
            RegionType = regionType;
            Id = id;
        }
    }
}