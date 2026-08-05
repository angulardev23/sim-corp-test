using BackendTest.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BackendTest.Api.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<PersonService>();
            services.AddScoped<ProductService>();
            services.AddScoped<PurchaseService>();
            services.AddScoped<PurchaseReportService>();

            return services;
        }
    }
}
