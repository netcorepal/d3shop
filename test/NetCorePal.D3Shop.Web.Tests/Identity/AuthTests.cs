using System.Net;
using System.Net.Http.Headers;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Infrastructure;
using NetCorePal.D3Shop.Web.Helper;

namespace NetCorePal.D3Shop.Web.Tests.Identity;

[Collection("web")]
public class AuthTests
{
    private readonly HttpClient _client;
    private readonly AdminUser _testUser = new("Test", "", "", [], [], "", 1, "");

    public AuthTests(MyWebApplicationFactory factory)
    {
        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _testUser.SetPassword(PasswordHasher.HashPassword(AppDefaultCredentials.Password));
            _testUser.SetSpecificPermissions([new AdminUserPermission("AdminUserAuth_Test_Get")]);
            db.AdminUsers.Add(_testUser);
            db.SaveChanges();
        }

        var clientFactory = factory.WithWebHostBuilder(builder => { builder.ConfigureServices(services => { }); });
        clientFactory.ClientOptions.AllowAutoRedirect = false; // 禁用自动重定向
        _client = clientFactory.CreateClient();
    }

    //[Fact]
    //public async Task Auth_Test()
    //{
    //    var response = await _client.GetAsync("/test/AdminUserAuthTest");
    //    Assert.True(response.StatusCode == HttpStatusCode.Redirect);
    //    var json = $$"""
    //                 {
    //                      "name": "{{_testUser.Name}}",
    //                      "password": "{{AppDefaultCredentials.Password}}"
    //                 }
    //                 """;
    //    var content = new StringContent(json);
    //    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
    //    response = await _client.PostAsync("api/AdminUserAccount/login", content);
    //    Assert.True(response.IsSuccessStatusCode);
    //    response = await _client.GetAsync("/test/AdminUserAuthTest");
    //    Assert.True(response.IsSuccessStatusCode);
    //    response = await _client.PostAsync("/test/AdminUserAuthTest", null);
    //    Assert.True(response.StatusCode == HttpStatusCode.Redirect);
    //}
}