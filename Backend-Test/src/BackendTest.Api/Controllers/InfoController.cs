using System.Reflection;
using BackendTest.Api.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BackendTest.Api.Controllers;

[ApiController]
[Route("api/info")]
public sealed class InfoController : ControllerBase
{
    [HttpGet]
    public ActionResult<ApplicationInfoResponse> Get()
    {
        var assembly = typeof(InfoController).Assembly;
        var serviceName = assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product ?? "unknown";
        var version = assembly.GetName().Version?.ToString(3) ?? "unknown";
        return Ok(new ApplicationInfoResponse(serviceName, version));
    }
}