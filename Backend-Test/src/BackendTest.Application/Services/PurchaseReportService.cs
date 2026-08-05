using System.Threading;
using System.Threading.Tasks;
using BackendTest.Application.Exceptions;
using BackendTest.Application.Repositories;
using BackendTest.Application.Reports;

namespace BackendTest.Application.Services;

public sealed class PurchaseReportService(
    IPurchaseReportRepository repository,
    IPurchaseReportFormatter formatter)
{
    public async Task<byte[]> GenerateAsync(int purchaseId, CancellationToken cancellationToken)
    {
        var report = await repository.FindByPurchaseIdAsync(purchaseId, cancellationToken)
            ?? throw new ResourceNotFoundException("Purchase", purchaseId);

        return formatter.Format(report);
    }
}
