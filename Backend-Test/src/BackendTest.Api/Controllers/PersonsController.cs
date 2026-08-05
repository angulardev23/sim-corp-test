using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BackendTest.Api.Contracts;
using BackendTest.Application.Models;
using BackendTest.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendTest.Api.Controllers;

[ApiController]
[Route("persons")]
public sealed class PersonsController(PersonService service) : ControllerBase
{
    [HttpGet("getAll")]
    public async Task<ActionResult<IReadOnlyList<PersonData>>> GetAll(CancellationToken cancellationToken) =>
        Ok(await service.GetAllAsync(cancellationToken));

    [HttpGet("get/{id:int}")]
    public async Task<ActionResult<PersonData>> GetById(int id, CancellationToken cancellationToken) =>
        Ok(await service.GetByIdAsync(id, cancellationToken));

    [HttpPost("add")]
    public async Task<ActionResult<PersonData>> Add(PersonContract person, CancellationToken cancellationToken)
    {
        var created = await service.AddAsync(person.ToApplicationModel(), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPost("update/{id:int}")]
    public async Task<ActionResult<PersonData>> Update(
        int id, PersonContract person, CancellationToken cancellationToken) =>
        Ok(await service.UpdateAsync(id, person.ToApplicationModel(), cancellationToken));

    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await service.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
