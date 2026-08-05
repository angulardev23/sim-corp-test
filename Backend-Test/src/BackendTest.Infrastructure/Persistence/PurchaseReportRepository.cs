using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BackendTest.Application.Models;
using BackendTest.Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BackendTest.Infrastructure.Persistence
{
    public sealed class PurchaseReportRepository : IPurchaseReportRepository
    {
        private readonly BackendTestDbContext _context;

        public PurchaseReportRepository(BackendTestDbContext context) => _context = context;

        public async Task<PurchaseReport?> FindByPurchaseIdAsync(
            int purchaseId,
            CancellationToken cancellationToken)
        {
            var purchase = await _context.Purchases
                .AsNoTracking()
                .Include(item => item.Customer)
                .Include(item => item.Products)
                .ThenInclude(item => item.Product)
                .SingleOrDefaultAsync(item => item.Id == purchaseId, cancellationToken);

            if (purchase is null)
            {
                return null;
            }

            var lines = purchase.Products
                .OrderBy(item => item.ProductId)
                .Select(item => new PurchaseReportLine(
                    item.ProductId,
                    item.Quantity,
                    item.Product.Name,
                    item.Product.Price))
                .ToArray();

            return new PurchaseReport(
                purchase.Id,
                $"{purchase.Customer.FirstName} {purchase.Customer.LastName}",
                lines);
        }
    }
}
