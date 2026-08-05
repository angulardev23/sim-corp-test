using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BackendTest.Domain.Entities;

namespace BackendTest.Application.Repositories;

public interface IPurchaseRepository
{
    Task<IReadOnlyList<Purchase>> GetAllAsync(CancellationToken cancellationToken);
    Task<Purchase?> FindByIdAsync(int id, CancellationToken cancellationToken);
    Task<Purchase?> FindFirstByCustomerIdAsync(int customerId, CancellationToken cancellationToken);
    Task AddAsync(Purchase purchase, CancellationToken cancellationToken);
    Task RemoveAsync(Purchase purchase, CancellationToken cancellationToken);
}
