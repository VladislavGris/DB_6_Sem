namespace Lab2.Models
{
    internal class ProductType : BaseEntity
    {
        public string Name { get; set; }

        public ProductType()
        {
            Name = "";
        }

        public ProductType(string name)
        {
            Name = name;
        }
    }
}
