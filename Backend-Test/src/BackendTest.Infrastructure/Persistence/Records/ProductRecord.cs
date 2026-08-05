namespace BackendTest.Infrastructure.Persistence.Records;

internal sealed class ProductRecord
{
    internal ProductRecord(int id, string name, string type, decimal price)
    {
        Id = id;
        Name = name;
        Type = type;
        Price = price;
    }

    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Type { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
}
