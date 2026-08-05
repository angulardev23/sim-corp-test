using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BackendTest.Domain.Entities;

namespace BackendTest.Application.Repositories;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken);
    Task<Product?> FindByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(Product product, CancellationToken cancellationToken);
    Task ReplaceAsync(Product product, CancellationToken cancellationToken);
    Task RemoveAsync(Product product, CancellationToken cancellationToken);
}
