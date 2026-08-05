using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BackendTest.Application.Exceptions;
using BackendTest.Application.Models;
using BackendTest.Application.Repositories;
using BackendTest.Domain.Entities;

namespace BackendTest.Application.Services
{
    public sealed class ProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository) => _repository = repository;

        public async Task<IReadOnlyList<ProductData>> GetAllAsync(CancellationToken cancellationToken) =>
            (await _repository.GetAllAsync(cancellationToken)).Select(ToData).ToArray();

        public async Task<ProductData> GetByIdAsync(int id, CancellationToken cancellationToken) =>
            ToData(await FindByIdAsync(id, cancellationToken));

        public async Task<ProductData> AddAsync(ProductData request, CancellationToken cancellationToken)
        {
            var product = ToEntity(request);
            await _repository.AddAsync(product, cancellationToken);
            return ToData(product);
        }

        public async Task<ProductData> UpdateAsync(int routeId, ProductData request, CancellationToken cancellationToken)
        {
            if (routeId != request.Id)
            {
                throw new RequestConflictException("Route id does not match the product's id.");
            }

            var existingProduct = await FindByIdAsync(routeId, cancellationToken);
            var product = ToEntity(request);
            await _repository.ReplaceAsync(product, cancellationToken);
            return ToData(product);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken) =>
            await _repository.RemoveAsync(await FindByIdAsync(id, cancellationToken), cancellationToken);

        private async Task<Product> FindByIdAsync(int id, CancellationToken cancellationToken) =>
            await _repository.FindByIdAsync(id, cancellationToken)
            ?? throw new ResourceNotFoundException("Product", id);

        private static Product ToEntity(ProductData product) =>
            new(product.Id, product.Name, product.Type, product.Price);

        private static ProductData ToData(Product product) =>
            new(product.Id, product.Name, product.Type, product.Price);
    }
}
