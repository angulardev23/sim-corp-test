using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BackendTest.Api.Contracts;
using BackendTest.Application.Models;
using Xunit;

namespace BackendTests.Api.IntegrationTests;

public sealed class ProductsControllerTests
{
    [Fact]
    public async Task GetAll_ReturnsSeededProducts()
    {
        using var factory = ApiTestHost.CreateFactory();
        using var client = factory.CreateTestClient();

        var products = await client.GetFromJsonAsync<ProductData[]>("/products/getAll");

        Assert.NotNull(products);
        Assert.Equal(10, products.Length);
        Assert.Contains(products, product => product.Id == 1 && product.Name == "Pipe Wrench");
    }

    [Fact]
    public async Task GetById_ReturnsRequestedProduct()
    {
        using var factory = ApiTestHost.CreateFactory();
        using var client = factory.CreateTestClient();

        var product = await client.GetFromJsonAsync<ProductData>("/products/get/1");

        Assert.NotNull(product);
        Assert.Equal(new ProductData(1, "Pipe Wrench", "Plumbing", 19.99m), product);
    }

    [Fact]
    public async Task Add_CreatesProductAndReturnsLocation()
    {
        using var factory = ApiTestHost.CreateFactory();
        using var client = factory.CreateTestClient();
        var request = new ProductContract
        {
            Id = 101,
            Name = "Multimeter",
            Type = "Electric",
            Price = 39.95m
        };

        var response = await client.PostAsJsonAsync("/products/add", request);
        var created = await response.Content.ReadFromJsonAsync<ProductData>();

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal("https://localhost/products/get/101", response.Headers.Location?.ToString());
        Assert.Equal(new ProductData(101, "Multimeter", "Electric", 39.95m), created);
    }

    [Fact]
    public async Task Update_ReplacesProduct()
    {
        using var factory = ApiTestHost.CreateFactory();
        using var client = factory.CreateTestClient();
        var request = new ProductContract
        {
            Id = 1,
            Name = "Large Pipe Wrench",
            Type = "Plumbing",
            Price = 29.99m
        };

        var response = await client.PostAsJsonAsync("/products/update/1", request);
        var updated = await response.Content.ReadFromJsonAsync<ProductData>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(new ProductData(1, "Large Pipe Wrench", "Plumbing", 29.99m), updated);
    }

    [Fact]
    public async Task Delete_RemovesProduct()
    {
        using var factory = ApiTestHost.CreateFactory();
        using var client = factory.CreateTestClient();
        await client.PostAsJsonAsync("/products/add", new ProductContract
        {
            Id = 101,
            Name = "Temporary Product",
            Type = "Test",
            Price = 1m
        });

        var response = await client.DeleteAsync("/products/delete/101");
        var getResponse = await client.GetAsync("/products/get/101");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}
