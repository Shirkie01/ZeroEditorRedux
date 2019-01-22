namespace SWBF2
{
    internal class Connection
    {
        public string Name { get; set; }

        public string Start { get; set; }
        public string End { get; set; }

        public int Flag { get; set; }
        public int DynamicGroup { get; set; }

        public Connection(string name)
        {
            Name = name;
        }
    }
}