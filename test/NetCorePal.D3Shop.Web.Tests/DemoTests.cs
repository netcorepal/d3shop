using NetCorePal.D3Shop.Infrastructure;
using NetCorePal.D3Shop.Web.Controllers;
using Microsoft.EntityFrameworkCore;
using Moq;
using NetCorePal.Context;
using NetCorePal.Extensions.AspNetCore;

namespace NetCorePal.D3Shop.Web.Tests
{
    [Collection("web")]
    public class DemoTests : IClassFixture<MyWebApplicationFactory>
    {
        private readonly MyWebApplicationFactory _factory;

        private readonly HttpClient _client;

        public DemoTests(MyWebApplicationFactory factory)
        {
            //using (var scope = factory.Services.CreateScope())
            //{
            //    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            //    db.Database.Migrate();
            //}

            _factory = factory;
            _client = factory.WithWebHostBuilder(builder => { builder.ConfigureServices(p => { }); }).CreateClient();
        }


        [Fact]
        public void HealthCheckTest()
        {
            var response = _client.GetAsync("/health").Result;
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task JsonTest()
        {
            var json = """
                       {
                         "id": "5",
                         "name": "myName",
                         "time": "2021-08-31T15:00:00"
                       }
                       """;
            var content = new StringContent(json);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await _client.PostAsync("/demo/json", content);
            Assert.True(response.IsSuccessStatusCode);
            var responseData = await response.Content.ReadAsStringAsync();
            Assert.NotNull(responseData);
            Assert.Contains("2021-08-31T15:00:00", responseData);
            Assert.Contains("\"name\":\"myName\"", responseData);
            Assert.Contains("\"id\":\"5\"", responseData);
        }

        [Fact]
        public async Task Json_ReadFromJson_Test()
        {
            var json = """
                       {
                         "id": "5",
                         "name": "myName",
                         "time": "2021-08-31 15:00:00"
                       }
                       """;
            var content = new StringContent(json);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await _client.PostAsync("/demo/json", content);
            Assert.True(response.IsSuccessStatusCode);
            var responseData = await response.Content.ReadFromNewtonsoftJsonAsync<ResponseData<JsonResponse>>();
            Assert.NotNull(responseData);
            Assert.Equal(DateTime.Parse("2021-08-31 15:00:00"), responseData.Data.Time);
            Assert.Equal("myName", responseData.Data.Name);
            Assert.Equal(5, responseData.Data.Id.Id);
        }

        [Fact]
        public async Task ValidatorTest()
        {
            var cmd = new ValidatorCommand("", 0);
            var response = await _client.PostAsNewtonsoftJsonAsync("/demo/validator", cmd);
            Assert.True(response.IsSuccessStatusCode);
            var responseData = await response.Content.ReadFromNewtonsoftJsonAsync<ValidatorResponseData>();
            Assert.NotNull(responseData);
            Assert.False(responseData.success);
            Assert.Equal("不能为空", responseData.message);
            Assert.Equal(400, responseData.code);
            Assert.NotNull(responseData.errorData);
            Assert.Equal(2, responseData.errorData.Count());
            var errors = responseData.errorData.ToList();
            Assert.Equal("不能为空", errors[0].errorMessage);
            Assert.Equal("code1", errors[0].errorCode);
            Assert.Equal("Name", errors[0].propertyName);
            Assert.Equal("价格必须在18-60之间", errors[1].errorMessage);
            Assert.Equal("code2", errors[1].errorCode);
            Assert.Equal("Price", errors[1].propertyName);
        }


        record ValidatorResponseData(bool success, string message, int code, IEnumerable<ErrorData>? errorData);

        record ErrorData(string errorCode, string errorMessage, string propertyName);


        [Fact]
        public async Task ContextTest()
        {
            var content = new StringContent("{ }");
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            content.Headers.Add(TenantContext.ContextKey, "t1");
            var response = await _factory.CreateClient().PostAsync("/demo/context", content);
            Assert.True(response.IsSuccessStatusCode);
            var responseData = await response.Content.ReadFromNewtonsoftJsonAsync<ResponseData<string>>();
            Assert.NotNull(responseData);
            Assert.Equal("", responseData.Data);
        }

        [Fact]
        public async Task LockTest()
        {
            var task1 = _client.GetAsync("/demo/lock");
            var task2 = _client.GetAsync("/demo/lock");
            await Task.WhenAll(task1, task2);
            Assert.True(task1.Result.IsSuccessStatusCode);
            Assert.True(task2.Result.IsSuccessStatusCode);
            var result1 = await task1.Result.Content.ReadFromNewtonsoftJsonAsync<ResponseData<bool>>();
            var result2 = await task2.Result.Content.ReadFromNewtonsoftJsonAsync<ResponseData<bool>>();
            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.False(result1.Data);
            Assert.False(result2.Data);
        }
    }
}