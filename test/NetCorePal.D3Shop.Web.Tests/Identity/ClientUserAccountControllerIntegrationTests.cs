using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using NetCorePal.D3Shop.Infrastructure;
using NetCorePal.D3Shop.Web.Controllers.Identity.Client.Requests;
using NetCorePal.D3Shop.Web.Helper;
using NetCorePal.Extensions.Dto;

namespace NetCorePal.D3Shop.Web.Tests.Identity;

[Collection("web")]
public class ClientUserAccountControllerIntegrationTests
{
    private readonly MyWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public ClientUserAccountControllerIntegrationTests(
        MyWebApplicationFactory factory )
    {
        _factory = factory;
        _client = _factory.WithWebHostBuilder(builder => { builder.ConfigureServices(_ => { }); })
            .CreateClient();
    }

    [Fact]
    public async Task Register_ValidRequest_ReturnsJwtToken()
    {
        // Arrange
        var request = new ClientUserRegisterRequest
        (
            "test_user",
            "avatar.png",
            "13800138000",
            "Test@123456",
            "test@example.com"
        );

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/ClientUserAccount/register", request);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ResponseData<string>>();
        Assert.NotNull(result?.Data);
        Assert.False(string.IsNullOrEmpty(result.Data));

        // 验证数据库是否创建了用户
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var user = await dbContext.ClientUsers
            .FirstOrDefaultAsync(u => u.Phone == request.Phone);
        Assert.NotNull(user);
        Assert.Equal(request.NickName, user.NickName);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsJwtToken()
    {
        // 先注册用户
        var registerRequest = new ClientUserRegisterRequest
        (
            "login_test",
            "avatar.png",
            "13800138001",
            "Test@123456",
            "login@test.com"
        );
        await _client.PostAsJsonAsync("/api/ClientUserAccount/register", registerRequest);

        // 登录请求
        var loginRequest = new ClientUserLoginRequest
        (
            "13800138001",
            "Test@123456",
            "1",
            "127.0.0.1",
            "xUnit"
        );

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/ClientUserAccount/login", loginRequest);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ResponseData<string>>();
        var token = result?.Data;
        Assert.NotNull(token);

        // 验证JWT有效性
        var tokenGenerator = _factory.Services.GetRequiredService<TokenGenerator>();
        var principal = tokenGenerator.GetPrincipalFromToken(token);
        Assert.NotNull(principal);
        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
        Assert.NotNull(userIdClaim);

        // 验证用户ID是否正确
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var user = await dbContext.ClientUsers
            .FirstOrDefaultAsync(u => u.Phone == loginRequest.Phone);
        Assert.Equal(userIdClaim.Value, user?.Id.ToString());
    }

    [Fact]
    public async Task Login_InvalidPassword_ReturnsUnauthorized()
    {
        // 注册用户
        var registerRequest = new ClientUserRegisterRequest
        (
            "nick",
            "avatar.png",
            "13800138002",
            "CorrectPassword",
            "invalid_test"
        );
        await _client.PostAsJsonAsync("/api/ClientUserAccount/register", registerRequest);

        // 使用错误密码登录
        var loginRequest = new ClientUserLoginRequest
        (
            "13800138002",
            "WrongPassword",
            "1",
            "127.0.0.1",
            "xUnit"
        );

        // Act & Assert
        var response = await _client.PostAsJsonAsync(
            "/api/ClientUserAccount/login", loginRequest);
        var result = await response.Content.ReadFromJsonAsync<ResponseData>();
        Assert.NotNull(result);
        Assert.Equal("用户名或密码错误", result.Message);
    }
}