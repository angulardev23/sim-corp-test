using System.Threading;
using System.Threading.Tasks;
using BackendTest.Application.Exceptions;
using BackendTest.Application.Models;
using BackendTest.Application.Repositories;
using BackendTest.Application.Reports;
using BackendTest.Application.Services;
using Xunit;

namespace BackendTests.Application.Tests
{
    public sealed class PurchaseReportServiceTests
    {
        [Fact]
        public async Task GenerateAsync_WhenPurchaseDoesNotExist_ThrowsResourceNotFoundException()
        {
            var service = new PurchaseReportService(
                new MissingPurchaseReportRepository(),
                new UnusedFormatter());

            await Assert.ThrowsAsync<ResourceNotFoundException>(() =>
                service.GenerateAsync(999, CancellationToken.None));
        }

        private sealed class MissingPurchaseReportRepository : IPurchaseReportRepository
        {
            public Task<PurchaseReport?> FindByPurchaseIdAsync(
                int purchaseId,
                CancellationToken cancellationToken) =>
                Task.FromResult<PurchaseReport?>(null);
        }

        private sealed class UnusedFormatter : IPurchaseReportFormatter
        {
            public byte[] Format(PurchaseReport report) =>
                throw new Xunit.Sdk.XunitException("Formatter should not be called for a missing purchase.");
        }
    }
}
