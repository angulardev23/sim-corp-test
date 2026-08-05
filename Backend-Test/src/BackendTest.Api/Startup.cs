using BackendTest.Api.Middleware;
using BackendTest.Application.Services;
using BackendTest.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace BackendTest.Api
{
    public sealed class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(options =>
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Backend Test API", Version = "v1" }));
            services.AddScoped<PersonService>();
            services.AddScoped<ProductService>();
            services.AddScoped<PurchaseService>();

            services.AddInfrastructure();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            app.ApplicationServices.InitializeInfrastructure();
            app.UseMiddleware<ApiExceptionMiddleware>();

            if (environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend Test API v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
