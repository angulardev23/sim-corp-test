using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BackendTest.Application.Models;

namespace BackendTest.Api.Contracts;

public sealed class PurchaseContract
{
    public int Id { get; init; }
    public int CustomerId { get; init; }

    [Required]
    public List<int> ProductId { get; init; } = new();

    public PurchaseData ToApplicationModel() => new(Id, CustomerId, ProductId);
}
