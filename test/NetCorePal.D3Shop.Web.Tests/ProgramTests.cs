using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using NetCorePal.Extensions.AspNetCore.Json;
using Xunit;

namespace NetCorePal.D3Shop.Web.Tests
{
    [Collection("web")]
    public class ProgramTests : IClassFixture<MyWebApplicationFactory>
    {
        private readonly MyWebApplicationFactory _factory;

        public ProgramTests(MyWebApplicationFactory factory)
        {
            _factory = factory;
        }


        [Fact]
        public void HealthCheckTest()
        {
            var client = _factory.CreateClient();
            var response = client.GetAsync("/health").Result;
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}