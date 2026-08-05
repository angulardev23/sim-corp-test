using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BackendTest.Api.Contracts;
using BackendTest.Application.Models;
using BackendTest.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendTest.Api.Controllers
{
    [ApiController]
    [Route("purchases")]
    public sealed class PurchasesController : ControllerBase
    {
        private readonly PurchaseService _service;

        public PurchasesController(PurchaseService service) => _service = service;

        [HttpGet("getAll")]
        public async Task<ActionResult<IReadOnlyList<PurchaseData>>> GetAll(CancellationToken cancellationToken) =>
            Ok(await _service.GetAllAsync(cancellationToken));

        [HttpGet("get/{id:int}")]
        public async Task<ActionResult<PurchaseData>> GetByCustomerId(int id, CancellationToken cancellationToken) =>
            Ok(await _service.GetFirstByCustomerIdAsync(id, cancellationToken));

        /// <summary>
        /// Reserved for the purchase CSV report task. This endpoint is intentionally not implemented.
        /// </summary>
        [HttpGet("get/{id:int}/report")]
        public Task<ActionResult<byte[]>> GetPurchaseReportById(int id) =>
            throw new NotImplementedException("Please implement me!");

        [HttpPost("add")]
        public async Task<ActionResult<PurchaseData>> Add(
            PurchaseContract purchase, CancellationToken cancellationToken)
        {
            var created = await _service.AddAsync(purchase.ToApplicationModel(), cancellationToken);
            return StatusCode(201, created);
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpDelete("delete/customer/{customerId:int}")]
        public async Task<IActionResult> DeleteFromCustomer(
            int customerId, CancellationToken cancellationToken)
        {
            await _service.DeleteFirstForCustomerAsync(customerId, cancellationToken);
            return NoContent();
        }
    }
}
