namespace Lab2.Models
{
    internal class Product : BaseEntity
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
        public int StorageId { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public double Weight { get; set; }

        public Product() { }

        public Product(int customerId, string customerName, int typeId, string typeName, string name, int count, double weight, int storageId)
        {
            CustomerId = customerId;
            CustomerName = customerName;
            ProductTypeId = typeId;
            ProductTypeName = typeName;
            StorageId = storageId;
            Name = name;
            Count = count;
            Weight = weight;
        }

        public Product(int id, int customerId, string customerName, int typeId, string typeName, string name, int count, double weight, int storageId) : this(customerId, customerName, typeId, typeName, name, count, weight, storageId)
        {
            Id = id;
        }
    }
}
