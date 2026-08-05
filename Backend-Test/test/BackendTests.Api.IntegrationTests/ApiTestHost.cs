using System;
using System.Net.Http;
using BackendTest.Api;
using BackendTest.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BackendTests.Api.IntegrationTests;

internal static class ApiTestHost
{
    public static WebApplicationFactory<Startup> CreateFactory()
    {
        var databaseName = $"BackendTests-{Guid.NewGuid()}";

        return new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbContextOptions<BackendTestDbContext>>();
                services.RemoveAll<BackendTestDbContext>();
                services.AddDbContext<BackendTestDbContext>(options =>
                    options.UseInMemoryDatabase(databaseName));
            }));
    }

    public static HttpClient CreateTestClient(this WebApplicationFactory<Startup> factory)
    {
        var client = factory.Server.CreateClient();
        client.BaseAddress = new Uri("https://localhost");
        return client;
    }
}
