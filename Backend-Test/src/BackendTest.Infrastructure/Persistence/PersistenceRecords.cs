using System.Collections.Generic;

namespace BackendTest.Infrastructure.Persistence
{
    internal sealed class PersonRecord
    {
        internal PersonRecord(int id, string firstName, string lastName, decimal yearOfBirth)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            YearOfBirth = yearOfBirth;
        }

        public int Id { get; private set; }
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public decimal YearOfBirth { get; private set; }
    }

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

    internal sealed class PurchaseRecord
    {
        internal PurchaseRecord(int id, int customerId)
        {
            Id = id;
            CustomerId = customerId;
        }

        public int Id { get; private set; }
        public int CustomerId { get; private set; }
        public PersonRecord Customer { get; private set; } = null!;
        public ICollection<PurchaseProductRecord> Products { get; } = new List<PurchaseProductRecord>();
    }

    internal sealed class PurchaseProductRecord
    {
        internal PurchaseProductRecord(int purchaseId, int productId, int quantity)
        {
            PurchaseId = purchaseId;
            ProductId = productId;
            Quantity = quantity;
        }

        public int PurchaseId { get; private set; }
        public PurchaseRecord Purchase { get; private set; } = null!;
        public int ProductId { get; private set; }
        public ProductRecord Product { get; private set; } = null!;
        public int Quantity { get; private set; }
    }
}
