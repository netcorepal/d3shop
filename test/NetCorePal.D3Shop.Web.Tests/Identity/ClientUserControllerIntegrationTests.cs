using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;
using NetCorePal.D3Shop.Infrastructure;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Client.Dto;
using NetCorePal.D3Shop.Web.Controllers.Identity.Client.Requests;
using NetCorePal.D3Shop.Web.Helper;
using NetCorePal.Extensions.Dto;

namespace NetCorePal.D3Shop.Web.Tests.Identity;

[Collection("web")]
public class ClientUserControllerIntegrationTests
{
    private readonly HttpClient _client;
    private readonly ApplicationDbContext _dbContext;
    private readonly ClientUser _testUser;

    public ClientUserControllerIntegrationTests(MyWebApplicationFactory factory)
    {
        _client = factory.WithWebHostBuilder(builder => { builder.ConfigureServices(_ => { }); })
            .CreateClient();

        var scope = factory.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        _testUser = CreateTestUser();
        var token = scope.ServiceProvider.GetRequiredService<TokenGenerator>()
            .GenerateJwtAsync([new Claim(ClaimTypes.NameIdentifier, _testUser.Id.ToString())]);

        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
    }

    private ClientUser CreateTestUser()
    {
        var (hash, salt) = NewPasswordHasher.HashPassword("password");
        var user = new ClientUser("Test User", "a", "1", hash, salt, "test@example.com");
        _dbContext.ClientUsers.Add(user);
        _dbContext.SaveChanges();
        return user;
    }

    [Fact]
    public async Task AddDeliveryAddress_Success()
    {
        // Arrange
        var request = new AddDeliveryAddressRequest(
            _testUser.Id,
            "Test Address",
            "Recipient",
            "13800138000",
            true
        );

        // Act
        var response = await _client.PostAsNewtonsoftJsonAsync("/api/ClientUser/AddDeliveryAddress", request);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ResponseData>();
        Assert.True(result!.Success);

        var user = await _dbContext.ClientUsers.AsNoTracking()
            .FirstAsync(u => u.Id == _testUser.Id);
        Assert.Single(user.DeliveryAddresses);
        var address = user.DeliveryAddresses.First();
        Assert.Equal("Test Address", address.Address);
        Assert.True(address.IsDefault);
    }

    [Fact]
    public async Task GetDeliveryAddresses_Success()
    {
        // Arrange
        _testUser.DeliveryAddresses.Clear();
        var newAddress = new UserDeliveryAddress(_testUser.Id, "Test Address", "Recipient", "13800138000", true);
        _testUser.DeliveryAddresses.Add(newAddress);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync($"/api/ClientUser/GetDeliveryAddresses?userId={_testUser.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content
            .ReadFromNewtonsoftJsonAsync<ResponseData<List<ClientUserDeliveryAddressInfo>>>();
        Assert.True(result!.Success);
        Assert.Single(result.Data);
        Assert.Equal("Test Address", result.Data[0].Address);
    }

    [Fact]
    public async Task RemoveDeliveryAddress_Success()
    {
        // Arrange
        _testUser.DeliveryAddresses.Clear();
        var newAddress = new UserDeliveryAddress(_testUser.Id, "Test Address", "Recipient", "13800138000", true);
        _testUser.DeliveryAddresses.Add(newAddress);
        await _dbContext.SaveChangesAsync();

        // Act
        var response =
            await _client.DeleteAsync(
                $"/api/ClientUser/RemoveDeliveryAddress?UserId={_testUser.Id}&DeliveryAddressId={newAddress.Id}");

        // Assert
        var result = await response.Content.ReadFromJsonAsync<ResponseData>();
        Assert.True(result!.Success);
        var updatedUser = await _dbContext.ClientUsers.AsNoTracking()
            .FirstAsync(u => u.Id == _testUser.Id);
        Assert.Empty(updatedUser.DeliveryAddresses);
    }

    [Fact]
    public async Task BindThirdPartyLogin_Success()
    {
        // Arrange
        var request = new BindThirdPartyLoginRequest(
            _testUser.Id,
            ThirdPartyProvider.WeChat,
            "test-app",
            "openid-123"
        );

        // Act
        var response = await _client.PostAsNewtonsoftJsonAsync("/api/ClientUser/BindThirdPartyLogin", request);

        // Assert
        response.EnsureSuccessStatusCode();
        var user = await _dbContext.ClientUsers.AsNoTracking().Include(clientUser => clientUser.ThirdPartyLogins)
            .FirstAsync(u => u.Id == _testUser.Id);
        Assert.Single(user.ThirdPartyLogins);
    }

    [Fact]
    public async Task GetThirdPartyLogin_Success()
    {
        // Arrange
        _testUser.ThirdPartyLogins.Clear();
        var newLogin = new UserThirdPartyLogin(_testUser.Id, ThirdPartyProvider.Qq, "get", "");
        _testUser.ThirdPartyLogins.Add(newLogin);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync("/api/ClientUser/GetThirdPartyLogins?userId=" + _testUser.Id);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content
            .ReadFromNewtonsoftJsonAsync<ResponseData<List<ClientUserThirdPartyLoginInfo>>>();
        Assert.True(result!.Success);
        Assert.Single(result.Data);
        Assert.Equal(ThirdPartyProvider.Qq, result.Data[0].ThirdPartyProvider);
    }

    [Fact]
    public async Task UnbindThirdPartyLogin_Success()
    {
        // Arrange
        _testUser.ThirdPartyLogins.Clear();
        var newLogin = new UserThirdPartyLogin(_testUser.Id, ThirdPartyProvider.Qq, "", "");
        _testUser.ThirdPartyLogins.Add(newLogin);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await _client.DeleteAsync("/api/ClientUser/UnbindThirdPartyLogin?UserId=" + _testUser.Id +
                                                 "&ThirdPartyLoginId=" + newLogin.Id);

        // Assert
        response.EnsureSuccessStatusCode();
        var user = await _dbContext.ClientUsers.AsNoTracking()
            .FirstAsync(u => u.Id == _testUser.Id);
        Assert.Empty(user.ThirdPartyLogins);
    }

    [Fact]
    public async Task EditPassword_Success()
    {
        // Arrange
        const string oldPassword = "password";
        const string newPassword = "new-password";

        var salt = _testUser.PasswordSalt;

        var request = new EditPasswordRequest(
            _testUser.Id,
            oldPassword,
            newPassword
        );

        // Act
        var response = await _client.PutAsNewtonsoftJsonAsync("/api/ClientUser/EditPassword", request);

        // Assert
        response.EnsureSuccessStatusCode();
        var updatedUser = await _dbContext.ClientUsers.AsNoTracking().SingleAsync(u => u.Id == _testUser.Id);
        var newHash = NewPasswordHasher.HashPassword(newPassword, salt);
        Assert.Equal(newHash, updatedUser.PasswordHash);
    }

    [Fact]
    public async Task DisableAndEnableUser_Success()
    {
        // Disable
        var disableRequest = new ClientUserDisableRequest(_testUser.Id, "Test reason");
        var disableResponse = await _client.PutAsNewtonsoftJsonAsync("/api/ClientUser/Disable", disableRequest);
        disableResponse.EnsureSuccessStatusCode();

        var disabledUser = await _dbContext.ClientUsers.AsNoTracking().SingleAsync(u => u.Id == _testUser.Id);
        Assert.True(disabledUser.IsDisabled);

        // Enable
        var enableResponse =
            await _client.PutAsNewtonsoftJsonAsync("/api/ClientUser/Enable", new ClientUserId(_testUser.Id.Id));
        enableResponse.EnsureSuccessStatusCode();

        var enabledUser = await _dbContext.ClientUsers.AsNoTracking().SingleAsync(u => u.Id == _testUser.Id);
        Assert.False(enabledUser.IsDisabled);
    }
}