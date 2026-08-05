using System;
using System.Threading;
using System.Threading.Tasks;
using BackendTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BackendTests.Infrastructure.Tests
{
    public sealed class PurchaseReportRepositoryTests
    {
        [Fact]
        public async Task FindByPurchaseIdAsync_ReturnsSeededCustomerAndProduct()
        {
            var options = new DbContextOptionsBuilder<BackendTestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            await using var context = new BackendTestDbContext(options);
            await context.Database.EnsureCreatedAsync();
            var repository = new PurchaseReportRepository(context);

            var report = await repository.FindByPurchaseIdAsync(1, CancellationToken.None);

            Assert.NotNull(report);
            Assert.Equal("John Doe", report.CustomerName);
            var line = Assert.Single(report.Lines);
            Assert.Equal(1, line.ProductId);
            Assert.Equal(1, line.Count);
            Assert.Equal("Pipe Wrench", line.ProductName);
            Assert.Equal(19.99m, line.Price);
        }
    }
}
