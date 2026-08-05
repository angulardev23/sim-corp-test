using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BackendTest.Api.Contracts;
using Xunit;

namespace BackendTests.Api.Tests
{
    public sealed class InfoControllerTests
    {
        [Fact]
        public async Task Get_ReturnsApplicationInformation()
        {
            using var factory = ApiTestHost.CreateFactory();
            using var client = factory.CreateTestClient();

            var response = await client.GetAsync("/api/info");
            var info = await response.Content.ReadFromJsonAsync<ApplicationInfoResponse>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(info);
            Assert.Equal("Backend Test API", info.Service);
            Assert.Equal("2.3.0", info.Version);
        }
    }
}
