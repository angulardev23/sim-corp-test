using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BackendTest.Application.Repositories;
using BackendTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendTest.Infrastructure.Persistence
{
    public sealed class PurchaseRepository : IPurchaseRepository
    {
        private readonly BackendTestDbContext _context;

        public PurchaseRepository(BackendTestDbContext context) => _context = context;

        public async Task<IReadOnlyList<Purchase>> GetAllAsync(CancellationToken cancellationToken) =>
            await _context.Purchases.AsNoTracking()
                .Select(purchase => new Purchase(
                    purchase.Id,
                    purchase.CustomerId,
                    purchase.Products.SelectMany(item =>
                        Enumerable.Repeat(item.ProductId, item.Quantity))))
                .ToArrayAsync(cancellationToken);

        public async Task<Purchase?> FindByIdAsync(int id, CancellationToken cancellationToken)
        {
            var purchase = await _context.Purchases.AsNoTracking()
                .Include(item => item.Products)
                .SingleOrDefaultAsync(item => item.Id == id, cancellationToken);
            return purchase is null ? null : ToDomain(purchase);
        }

        public async Task<Purchase?> FindFirstByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            var purchase = await _context.Purchases.AsNoTracking()
                .Include(item => item.Products)
                .FirstOrDefaultAsync(item => item.CustomerId == customerId, cancellationToken);
            return purchase is null ? null : ToDomain(purchase);
        }

        public async Task AddAsync(Purchase purchase, CancellationToken cancellationToken)
        {
            _context.Purchases.Add(ToRecord(purchase));
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveAsync(Purchase purchase, CancellationToken cancellationToken)
        {
            var record = await _context.Purchases
                .Include(item => item.Products)
                .SingleAsync(item => item.Id == purchase.Id, cancellationToken);
            _context.Purchases.Remove(record);
            await _context.SaveChangesAsync(cancellationToken);
        }

        private static Purchase ToDomain(PurchaseRecord purchase) => new(
            purchase.Id,
            purchase.CustomerId,
            purchase.Products.SelectMany(item => Enumerable.Repeat(item.ProductId, item.Quantity)));

        private static PurchaseRecord ToRecord(Purchase purchase)
        {
            var record = new PurchaseRecord(purchase.Id, purchase.CustomerId);

            foreach (var product in purchase.ProductIds.GroupBy(productId => productId))
            {
                record.Products.Add(new PurchaseProductRecord(purchase.Id, product.Key, product.Count()));
            }

            return record;
        }
    }
}
