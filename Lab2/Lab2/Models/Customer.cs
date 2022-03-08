namespace Lab2.Models
{
    internal class Customer : BaseEntity
    {
        public string Name { get; set; }
        public string ContactData { get; set; }

        public Customer()
        {
            Name = "";
            ContactData = "";
        }

        public Customer(string name, string contactData)
        {
            Name = name;
            ContactData = contactData;
        }

        public Customer(int id, string name, string contactData)
        {
            base.Id = id;
            Name = name;
            ContactData = contactData;
        }
    }
}
