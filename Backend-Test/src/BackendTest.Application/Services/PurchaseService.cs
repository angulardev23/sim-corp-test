using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BackendTest.Application.Exceptions;
using BackendTest.Application.Models;
using BackendTest.Application.Repositories;
using BackendTest.Domain.Entities;

namespace BackendTest.Application.Services;

public sealed class PurchaseService
{
    private readonly IPurchaseRepository _repository;

    public PurchaseService(IPurchaseRepository repository) => _repository = repository;

    public async Task<IReadOnlyList<PurchaseData>> GetAllAsync(CancellationToken cancellationToken) =>
        (await _repository.GetAllAsync(cancellationToken)).Select(ToData).ToArray();

    public async Task<PurchaseData> GetFirstByCustomerIdAsync(int customerId, CancellationToken cancellationToken) =>
        ToData(await FindFirstByCustomerIdAsync(customerId, cancellationToken));

    public async Task<PurchaseData> AddAsync(PurchaseData request, CancellationToken cancellationToken)
    {
        var purchase = new Purchase(request.Id, request.CustomerId, request.ProductId);
        await _repository.AddAsync(purchase, cancellationToken);
        return ToData(purchase);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var purchase = await _repository.FindByIdAsync(id, cancellationToken)
            ?? throw new ResourceNotFoundException("Purchase", id);
        await _repository.RemoveAsync(purchase, cancellationToken);
    }

    public async Task DeleteFirstForCustomerAsync(int customerId, CancellationToken cancellationToken) =>
        await _repository.RemoveAsync(
            await FindFirstByCustomerIdAsync(customerId, cancellationToken), cancellationToken);

    private async Task<Purchase> FindFirstByCustomerIdAsync(int customerId, CancellationToken cancellationToken) =>
        await _repository.FindFirstByCustomerIdAsync(customerId, cancellationToken)
        ?? throw new ResourceNotFoundException("Purchase for customer", customerId);

    private static PurchaseData ToData(Purchase purchase) =>
        new(purchase.Id, purchase.CustomerId, purchase.ProductIds);
}
