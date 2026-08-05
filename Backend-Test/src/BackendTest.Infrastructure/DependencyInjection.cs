using System;
using BackendTest.Application.Repositories;
using BackendTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BackendTest.Infrastructure
{
    public static class DependencyInjection
    {
        private const string DatabaseName = "BackendTest";

        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<BackendTestDbContext>(options => options.UseInMemoryDatabase(DatabaseName));
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            return services;
        }

        public static void InitializeInfrastructure(this IServiceProvider services)
        {
            using var scope = services.CreateScope();
            scope.ServiceProvider.GetRequiredService<BackendTestDbContext>().Database.EnsureCreated();
        }
    }
}
