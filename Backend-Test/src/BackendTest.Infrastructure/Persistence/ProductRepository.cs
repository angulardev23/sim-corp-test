using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BackendTest.Application.Repositories;
using BackendTest.Domain.Entities;
using BackendTest.Infrastructure.Persistence.Records;
using Microsoft.EntityFrameworkCore;

namespace BackendTest.Infrastructure.Persistence;

public sealed class ProductRepository : IProductRepository
{
    private readonly BackendTestDbContext _context;

    public ProductRepository(BackendTestDbContext context) => _context = context;

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken) =>
        await _context.Products.AsNoTracking()
            .Select(product => new Product(product.Id, product.Name, product.Type, product.Price))
            .ToArrayAsync(cancellationToken);

    public async Task<Product?> FindByIdAsync(int id, CancellationToken cancellationToken)
    {
        var product = await _context.Products.AsNoTracking()
            .SingleOrDefaultAsync(item => item.Id == id, cancellationToken);
        return product is null ? null : new Product(product.Id, product.Name, product.Type, product.Price);
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken)
    {
        _context.Products.Add(ToRecord(product));
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task ReplaceAsync(Product product, CancellationToken cancellationToken)
    {
        _context.Products.Update(ToRecord(product));
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(Product product, CancellationToken cancellationToken)
    {
        _context.Products.Remove(ToRecord(product));
        await _context.SaveChangesAsync(cancellationToken);
    }

    private static ProductRecord ToRecord(Product product) =>
        new(product.Id, product.Name, product.Type, product.Price);
}
