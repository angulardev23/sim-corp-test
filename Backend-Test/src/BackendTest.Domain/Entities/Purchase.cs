using System.Collections.Generic;
using System.Linq;

namespace BackendTest.Domain.Entities
{
    public sealed class Purchase
    {
        public Purchase(int id, int customerId, IEnumerable<int> productIds)
        {
            Id = id;
            CustomerId = customerId;
            ProductIds = productIds.ToArray();
        }

        public int Id { get; }
        public int CustomerId { get; }
        public IReadOnlyList<int> ProductIds { get; }
    }
}
