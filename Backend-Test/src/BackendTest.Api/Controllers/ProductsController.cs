using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BackendTest.Api.Contracts;
using BackendTest.Application.Models;
using BackendTest.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendTest.Api.Controllers;

[ApiController]
[Route("products")]
public sealed class ProductsController : ControllerBase
{
    private readonly ProductService _service;

    public ProductsController(ProductService service) => _service = service;

    [HttpGet("getAll")]
    public async Task<ActionResult<IReadOnlyList<ProductData>>> GetAll(CancellationToken cancellationToken) =>
        Ok(await _service.GetAllAsync(cancellationToken));

    [HttpGet("get/{id:int}")]
    public async Task<ActionResult<ProductData>> GetById(int id, CancellationToken cancellationToken) =>
        Ok(await _service.GetByIdAsync(id, cancellationToken));

    [HttpPost("add")]
    public async Task<ActionResult<ProductData>> Add(ProductContract product, CancellationToken cancellationToken)
    {
        var created = await _service.AddAsync(product.ToApplicationModel(), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPost("update/{id:int}")]
    public async Task<ActionResult<ProductData>> Update(
        int id, ProductContract product, CancellationToken cancellationToken) =>
        Ok(await _service.UpdateAsync(id, product.ToApplicationModel(), cancellationToken));

    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
