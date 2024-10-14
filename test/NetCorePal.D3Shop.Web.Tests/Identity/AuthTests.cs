using NetCorePal.D3Shop.Web.Controllers.Identity.Responses;
using NetCorePal.Extensions.AspNetCore;
using System.Net;
using System.Net.Http.Headers;

namespace NetCorePal.D3Shop.Web.Tests.Identity;

[Collection("web")]
public class AuthTests : IClassFixture<MyWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthTests(MyWebApplicationFactory factory)
    {
        _client = factory.WithWebHostBuilder(builder => { builder.ConfigureServices(p => { }); }).CreateClient();
    }

    private string _token = string.Empty;

    [Fact]
    public async Task Login_Test()
    {
        var json = """
                   {
                        "name": "Z_jie",
                        "password": "admin@001"
                   }
                   """;
        var content = new StringContent(json);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var response = await _client.PostAsync("/token/login", content);
        Assert.True(response.IsSuccessStatusCode);
        var responseData = await response.Content.ReadFromNewtonsoftJsonAsync<ResponseData<TokenResponse>>();
        Assert.NotNull(responseData);
        _token = responseData.Data.Token;
        Assert.NotNull(_token);
    }

    [Fact]
    public async Task Auth_Test()
    {
        await Login_Test();
        var response = await _client.GetAsync("test/auth");
        Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        response = await _client.GetAsync("test/auth");
        Assert.True(response.IsSuccessStatusCode);
        response = await _client.PostAsync("test/auth", null);
        Assert.True(response.StatusCode == HttpStatusCode.Forbidden);
    }
}