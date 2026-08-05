using System.Collections.Generic;
using System.Linq;

namespace BackendTest.Domain.Entities;

public sealed class Purchase(int id, int customerId, IEnumerable<int> productIds)
{
    public int Id { get; } = id;
    public int CustomerId { get; } = customerId;
    public IReadOnlyList<int> ProductIds { get; } = productIds.ToArray();
}
