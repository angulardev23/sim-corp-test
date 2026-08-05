using System.Collections.Generic;

namespace BackendTest.Infrastructure.Persistence
{
    internal sealed class PersonRecord
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public decimal YearOfBirth { get; set; }
    }

    internal sealed class ProductRecord
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public double Price { get; set; }
    }

    internal sealed class PurchaseRecord
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public ICollection<PurchaseProductRecord> Products { get; set; } = new List<PurchaseProductRecord>();
    }

    internal sealed class PurchaseProductRecord
    {
        public int PurchaseId { get; set; }
        public PurchaseRecord Purchase { get; set; } = null!;
        public int ProductId { get; set; }
        public ProductRecord Product { get; set; } = null!;
    }
}
