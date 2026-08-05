using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace BackendTest.Api.Controllers
{
    [ApiController] // TODO rething the controller
    [Route("environment")]
    public sealed class EnvironmentController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly AssemblyName _assemblyName;

        public EnvironmentController(IWebHostEnvironment environment)
        {
            _environment = environment;
            _assemblyName = Assembly.GetEntryAssembly()!.GetName();
        }

        [HttpGet("isproduction")]
        public ActionResult<bool> GetIsProduction() => _environment.IsProduction();

        [HttpGet("apiversion")]
        public ActionResult<string> GetApiVersion()
        {
            var version = _assemblyName.Version;
            return $"Api Version is {version?.Major}.{version?.Minor}";
        }

        [HttpGet("/api/info")]
        public IActionResult GetApplicationInfo() => Ok(new
        {
            service = "Backend Test API",
            version = _assemblyName.Version?.ToString(3) ?? "unknown"
        });
    }
}
