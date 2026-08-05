using System.Threading;
using System.Threading.Tasks;
using BackendTest.Application.Exceptions;
using BackendTest.Application.Repositories;
using BackendTest.Application.Reports;

namespace BackendTest.Application.Services
{
    public sealed class PurchaseReportService
    {
        private readonly IPurchaseReportRepository _repository;
        private readonly IPurchaseReportFormatter _formatter;

        public PurchaseReportService(
            IPurchaseReportRepository repository,
            IPurchaseReportFormatter formatter)
        {
            _repository = repository;
            _formatter = formatter;
        }

        public async Task<byte[]> GenerateAsync(int purchaseId, CancellationToken cancellationToken)
        {
            var report = await _repository.FindByPurchaseIdAsync(purchaseId, cancellationToken)
                ?? throw new ResourceNotFoundException("Purchase", purchaseId);

            return _formatter.Format(report);
        }
    }
}
