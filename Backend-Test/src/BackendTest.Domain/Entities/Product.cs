namespace BackendTest.Domain.Entities
{
    public sealed class Product
    {
        public Product(int id, string name, string type, double price)
        {
            Id = id;
            Name = name;
            Type = type;
            Price = price;
        }

        public int Id { get; }
        public string Name { get; }
        public string Type { get; }
        public double Price { get; }
    }
}
