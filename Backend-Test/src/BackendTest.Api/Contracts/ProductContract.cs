using System.ComponentModel.DataAnnotations;
using BackendTest.Application.Models;

namespace BackendTest.Api.Contracts;

public sealed class ProductContract
{
    public int Id { get; init; }

    [Required]
    public string Name { get; init; } = string.Empty;

    [Required]
    public string Type { get; init; } = string.Empty;

    public decimal Price { get; init; }

    public ProductData ToApplicationModel() => new(Id, Name, Type, Price);
}
