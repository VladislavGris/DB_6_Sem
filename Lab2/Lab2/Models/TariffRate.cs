namespace Lab2.Models
{
    internal class TariffRate : BaseEntity
    {
        public int StorageId { get; set; }
        public decimal Price { get; set; }
        public float WeightFrom { get; set; }
        public float WeightTo { get; set; }
    }
}
