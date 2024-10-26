using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Infrastructure;
using NetCorePal.D3Shop.Web.Controllers.Identity.Responses;
using NetCorePal.Extensions.AspNetCore;
using System.Net;
using System.Net.Http.Headers;
using NetCorePal.D3Shop.Web.Helper;

namespace NetCorePal.D3Shop.Web.Tests.Identity;

[Collection("web")]
public class AuthTests : IClassFixture<MyWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly AdminUser _testUser = new("Test", "", "", []);

    public AuthTests(MyWebApplicationFactory factory)
    {
        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _testUser.SetPassword(PasswordHasher.HashPassword(AppDefaultCredentials.Password));
            _testUser.SetSpecificPermissions([new AdminUserPermission("AdminUserAuth_Test_Get", "")]);
            db.AdminUsers.Add(_testUser);
            db.SaveChanges();
        }

        _client = factory.WithWebHostBuilder(builder => { builder.ConfigureServices(p => { }); }).CreateClient();
    }

    private string _token = string.Empty;

    [Fact]
    public async Task Login_Test()
    {
        var json = $$"""
                   {
                        "name": "{{_testUser.Name}}",
                        "password": "{{AppDefaultCredentials.Password}}"
                   }
                   """;
        var content = new StringContent(json);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var response = await _client.PostAsync("api/AdminUserToken/login", content);
        Assert.True(response.IsSuccessStatusCode);
        var responseData = await response.Content.ReadFromNewtonsoftJsonAsync<ResponseData<AminUserTokenResponse>>();
        Assert.NotNull(responseData);
        _token = responseData.Data.Token;
        Assert.NotNull(_token);
    }

    [Fact]
    public async Task Auth_Test()
    {
        if (string.IsNullOrEmpty(_token))
            await Login_Test();
        var response = await _client.GetAsync("/test/AdminUserAuthTest");
        Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        response = await _client.GetAsync("/test/AdminUserAuthTest");
        Assert.True(response.IsSuccessStatusCode);
        response = await _client.PostAsync("/test/AdminUserAuthTest", null);
        Assert.True(response.StatusCode == HttpStatusCode.Forbidden);
    }
}