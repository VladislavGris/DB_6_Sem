namespace Lab2.Models
{
    internal class Storage : BaseEntity
    {
        public string Location { get; set; }
        public int Capacity { get; set; }
        public double FreeSpace { get; set; }

        public Storage()
        {
            Location = "";
            Capacity = 0;
            FreeSpace = 0;
        }

        public Storage(string location, int capacity, double freeSpace)
        {
            Location = location;
            Capacity = capacity;
            FreeSpace = freeSpace;
        }

        public Storage(int id, string location, int capacity, double freeSpace):this(location, capacity, freeSpace)
        {
            Id = id;
        }
    }
}
