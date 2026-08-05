using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BackendTest.Api.Contracts;
using BackendTest.Application.Models;
using Xunit;

namespace BackendTests.Api.Tests
{
    public sealed class PurchasesControllerTests
    {
        [Fact]
        public async Task GetAll_ReturnsSeededPurchases()
        {
            using var factory = ApiTestHost.CreateFactory();
            using var client = factory.CreateTestClient();

            var purchases = await client.GetFromJsonAsync<PurchaseData[]>("/purchases/getAll");

            Assert.NotNull(purchases);
            Assert.Equal(39, purchases.Length);
            Assert.Contains(purchases, purchase => purchase.Id == 1 && purchase.CustomerId == 1);
        }

        [Fact]
        public async Task GetByCustomerId_ReturnsFirstCustomerPurchase()
        {
            using var factory = ApiTestHost.CreateFactory();
            using var client = factory.CreateTestClient();

            var purchase = await client.GetFromJsonAsync<PurchaseData>("/purchases/get/1");

            Assert.NotNull(purchase);
            Assert.Equal(1, purchase.CustomerId);
            Assert.NotEmpty(purchase.ProductId);
        }

        [Fact]
        public async Task GetReport_ReturnsCsvAttachment()
        {
            using var factory = ApiTestHost.CreateFactory();
            using var client = factory.CreateTestClient();

            var response = await client.GetAsync("/purchases/get/1/report");
            var csv = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("text/csv", response.Content.Headers.ContentType?.MediaType);
            Assert.Equal("utf-8", response.Content.Headers.ContentType?.CharSet);
            Assert.Equal("purchase-1-report.csv", response.Content.Headers.ContentDisposition?.FileName);
            Assert.Contains("CustomerName:;John Doe", csv);
            Assert.Contains("1;1;Pipe Wrench;19,99", csv);
        }

        [Fact]
        public async Task Add_CreatesPurchase()
        {
            using var factory = ApiTestHost.CreateFactory();
            using var client = factory.CreateTestClient();
            var request = new PurchaseContract
            {
                Id = 101,
                CustomerId = 10,
                ProductId = { 1, 2 }
            };

            var response = await client.PostAsJsonAsync("/purchases/add", request);
            var created = await response.Content.ReadFromJsonAsync<PurchaseData>();

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(created);
            Assert.Equal(101, created.Id);
            Assert.Equal(10, created.CustomerId);
            Assert.Equal(new[] { 1, 2 }, created.ProductId);
        }

        [Fact]
        public async Task DeleteById_RemovesPurchase()
        {
            using var factory = ApiTestHost.CreateFactory();
            using var client = factory.CreateTestClient();
            await AddPurchaseAsync(client, 101, 10);

            var response = await client.DeleteAsync("/purchases/delete/101");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            var purchases = await client.GetFromJsonAsync<PurchaseData[]>("/purchases/getAll");
            Assert.DoesNotContain(purchases!, purchase => purchase.Id == 101);
        }

        [Fact]
        public async Task DeleteByCustomerId_RemovesFirstCustomerPurchase()
        {
            using var factory = ApiTestHost.CreateFactory();
            using var client = factory.CreateTestClient();
            await AddPurchaseAsync(client, 101, 10);

            var response = await client.DeleteAsync("/purchases/delete/customer/10");
            var getResponse = await client.GetAsync("/purchases/get/10");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        private static async Task AddPurchaseAsync(System.Net.Http.HttpClient client, int id, int customerId)
        {
            var response = await client.PostAsJsonAsync("/purchases/add", new PurchaseContract
            {
                Id = id,
                CustomerId = customerId,
                ProductId = { 1 }
            });
            response.EnsureSuccessStatusCode();
        }
    }
}
