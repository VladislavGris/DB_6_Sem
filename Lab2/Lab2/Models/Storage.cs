namespace Lab2.Models
{
    internal class Storage : BaseEntity
    {
        public string Location { get; set; }
        public int Capacity { get; set; }
        public float FreeSpace { get; set; }

        public Storage()
        {
            Location = "";
            Capacity = 0;
            FreeSpace = 0;
        }

        public Storage(string location, int capacity, float freeSpace)
        {
            Location = location;
            Capacity = capacity;
            FreeSpace = freeSpace;
        }
    }
}
