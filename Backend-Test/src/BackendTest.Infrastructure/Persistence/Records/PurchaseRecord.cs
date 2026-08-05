using System.Collections.Generic;
using BackendTest.Infrastructure.Persistence.Records;

namespace BackendTest.Infrastructure.Persistence;

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
