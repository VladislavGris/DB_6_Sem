namespace Lab2.Models
{
    internal class Product : BaseEntity
    {
        public int CustomerId { get; set; }
        public int ProductTypeId { get; set; }
        public int StorageId { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public float Weight { get; set; }

        public Product()
        {
            CustomerId = 0;
            ProductTypeId = 0;
            StorageId = 0;
            Name = "";
            Count = 0;
            Weight = 0;
        }

        public Product(int customerId, int typeId, int storageId, string name, int count, float weight)
        {
            CustomerId = customerId;
            ProductTypeId = typeId;
            StorageId = storageId;
            Name = name;
            Count = count;
            Weight = weight;
        }
    }
}
