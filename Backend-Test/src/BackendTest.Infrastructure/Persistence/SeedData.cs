using System.Linq;

namespace BackendTest.Infrastructure.Persistence
{
    internal static class SeedData
    {
        private static readonly (int Id, int CustomerId, int ProductId)[] PurchaseValues =
        {
            (1, 1, 1), (2, 1, 2), (3, 1, 3), (4, 2, 4), (5, 2, 5), (6, 3, 6),
            (7, 7, 7), (8, 7, 8), (9, 4, 9), (10, 4, 10), (11, 4, 4), (12, 4, 8),
            (13, 8, 8), (14, 8, 2), (15, 5, 1), (16, 5, 6), (17, 8, 5), (18, 1, 4),
            (19, 2, 6), (20, 3, 10), (21, 4, 3), (22, 5, 1), (23, 1, 6), (24, 2, 10),
            (25, 3, 7), (26, 4, 1), (27, 5, 6), (28, 1, 10), (29, 2, 7), (30, 3, 1),
            (31, 4, 6), (32, 5, 10), (33, 1, 7), (34, 2, 1), (35, 3, 6), (36, 4, 10),
            (37, 6, 1), (38, 6, 4), (39, 6, 7)
        };

        internal static PersonRecord[] People() => new[]
        {
            Person(1, "John", "Doe", 1980), Person(2, "Jane", "Doe", 1985),
            Person(3, "Bob", "Smith", 1990), Person(4, "Alice", "Johnson", 1995),
            Person(5, "Mike", "Brown", 1982), Person(6, "Samantha", "Davis", 1987),
            Person(7, "David", "Wilson", 1992), Person(8, "Emily", "Taylor", 1997),
            Person(9, "Chris", "Anderson", 1984), Person(10, "Jessica", "Thomas", 1989)
        };

        internal static ProductRecord[] Products() => new[]
        {
            Product(1, "Pipe Wrench", "Plumbing", 19.99m), Product(2, "Electric Drill", "Electric", 49.99m),
            Product(3, "Garden Hose", "Gardening", 4.99m), Product(4, "Toilet Plunger", "Plumbing", 1.49m),
            Product(5, "Electric Screwdriver", "Electric", 29.99m),
            Product(6, "Garden Shovel", "Gardening", 14.99m), Product(7, "Faucet", "Plumbing", 24.99m),
            Product(8, "Electric Saw", "Electric", 89.99m), Product(9, "Garden Gloves", "Gardening", 9.99m),
            Product(10, "Pipe Cutter", "Plumbing", 12.99m)
        };

        internal static PurchaseRecord[] Purchases() => PurchaseValues
            .Select(value => new PurchaseRecord(value.Id, value.CustomerId))
            .ToArray();

        internal static PurchaseProductRecord[] PurchaseProducts() => PurchaseValues
            .Select(value => new PurchaseProductRecord(value.Id, value.ProductId, quantity: 1))
            .ToArray();

        private static PersonRecord Person(int id, string firstName, string lastName, decimal year) =>
            new(id, firstName, lastName, year);

        private static ProductRecord Product(int id, string name, string type, decimal price) =>
            new(id, name, type, price);

    }
}
