using System.Threading;
using System.Threading.Tasks;
using BackendTest.Application.Models;

namespace BackendTest.Application.Repositories
{
    public interface IPurchaseReportRepository
    {
        Task<PurchaseReport?> FindByPurchaseIdAsync(int purchaseId, CancellationToken cancellationToken);
    }
}
