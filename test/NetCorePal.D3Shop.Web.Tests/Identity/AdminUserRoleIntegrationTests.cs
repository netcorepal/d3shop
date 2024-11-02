using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Web.Controllers.Identity.Requests;
using NetCorePal.D3Shop.Web.Controllers.Identity.Responses;
using NetCorePal.Extensions.AspNetCore;
using System.Net.Http.Json;
using NetCorePal.D3Shop.Admin.Shared.Permission;

namespace NetCorePal.D3Shop.Web.Tests.Identity;

[Collection("web")]
public class AdminUserRoleIntegrationTests : IClassFixture<MyWebApplicationFactory>
{
    private readonly HttpClient _client;
    public AdminUserRoleIntegrationTests(MyWebApplicationFactory factory)
    {
        _client = factory.WithWebHostBuilder(builder => { builder.ConfigureServices(_ => { }); })
            .CreateClient();

        var configuration = factory.Services.GetRequiredService<IConfiguration>();
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Helpers.GenerateEncryptedToken(configuration));

    }

    /// <summary>
    /// 创建测试用户
    /// </summary>
    /// <param name="name"></param>
    /// <param name="roleIds"></param>
    /// <returns></returns>
    private async Task<AdminUserId> CreateTestAdminUser(string name, IEnumerable<RoleId> roleIds)
    {
        // Arrange
        var createAdminUserRequest = new CreateAdminUserRequest(name, "1", "123", roleIds);

        // Act - 期望用户创建成功
        var response = await _client.PostAsNewtonsoftJsonAsync("/api/AdminUser/CreateAdminUser", createAdminUserRequest);

        // Assert - 创建成功
        Assert.True(response.IsSuccessStatusCode);
        var responseData = await response.Content.ReadFromNewtonsoftJsonAsync<ResponseData<AdminUserId>>();
        Assert.NotNull(responseData);
        Assert.NotEqual(0, responseData.Data.Id); // 验证返回的用户ID不是默认值

        return responseData.Data;
    }

    /// <summary>
    /// 查询用户测试
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task CreateAdminUser_And_QueryAdminUserByVariousMethods_ShouldSucceed()
    {
        var testUserName = "TestQueryUser";
        var testUserId = await CreateTestAdminUser(testUserName, []);

        // Act 2: Get all admin users
        var allUsersResponse = await _client.GetAsync("/api/AdminUser/GetAllAdminUsers");
        allUsersResponse.EnsureSuccessStatusCode(); // 确保返回 200 OK

        var allUsersData = await allUsersResponse.Content.ReadFromNewtonsoftJsonAsync<ResponseData<IEnumerable<AdminUserResponse>>>();
        Assert.NotNull(allUsersData);
        Assert.NotEmpty(allUsersData.Data); // 验证返回的用户列表不为空

        // Act 3: Get admin users by condition
        var conditionResponse = await _client.GetAsync($"/api/AdminUser/GetAdminUsersByCondition?Name={testUserName}");
        conditionResponse.EnsureSuccessStatusCode(); // 确保返回 200 OK
        var conditionData = await conditionResponse.Content.ReadFromNewtonsoftJsonAsync<ResponseData<IEnumerable<AdminUserResponse>>>();
        Assert.NotNull(conditionData);
        Assert.All(conditionData.Data, user => Assert.Contains(testUserName, user.Name)); // 验证返回的用户符合条件

        // Act 4: Get the created user by ID
        var getUserResponse = await _client.GetAsync($"/api/AdminUser/GetAdminUserById/{testUserId.Id}");
        getUserResponse.EnsureSuccessStatusCode(); // 确保返回 200 OK
        var getUserData = await getUserResponse.Content.ReadFromNewtonsoftJsonAsync<ResponseData<AdminUserResponse>>();
        Assert.NotNull(getUserData);
        Assert.Equal(testUserId, getUserData.Data.Id); // 验证返回的用户 ID 符合请求
    }

    /// <summary>
    /// 创建测试角色
    /// </summary>
    /// <param name="roleName"></param>
    /// <param name="roleDescription"></param>
    /// <param name="permissionCodes"></param>
    /// <returns></returns>
    private async Task<RoleId> CreateTestRoleAsync(string roleName, string roleDescription, IEnumerable<string> permissionCodes)
    {
        // Arrange
        var createRoleRequest = new CreateRoleRequest(roleName, roleDescription, permissionCodes);

        // Act
        var response = await _client.PostAsJsonAsync("api/Role/CreateRole", createRoleRequest);
        response.EnsureSuccessStatusCode(); // 确保返回 200 OK

        var responseData = await response.Content.ReadFromNewtonsoftJsonAsync<ResponseData<RoleId>>();
        Assert.NotNull(responseData); // 确保响应不为空

        return responseData.Data; // 返回创建的 RoleId
    }

    /// <summary>
    /// 测试更新用户角色
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task UpdateAdminUserRoles_ShouldUpdateRolesSuccessfully()
    {
        // Step 1: 创建测试角色
        var permissionCodes = Permissions.AllPermissions.Take(2).Select(p => p.Code).ToList();
        var roleId = await CreateTestRoleAsync("TestRole", "Description of the test role", permissionCodes);

        // Step 2: 创建测试用户并分配角色
        const string testUserName = "TestUpdateRolesUsers";
        var testUserId = await CreateTestAdminUser(testUserName, [roleId]);

        // Step 3: 验证用户初始角色和权限
        var getUserDataResponse = await _client.GetAsync($"/api/AdminUser/GetAdminUserById/{testUserId}");
        getUserDataResponse.EnsureSuccessStatusCode();
        var userData = await getUserDataResponse.Content.ReadFromNewtonsoftJsonAsync<ResponseData<AdminUserResponse>>();
        Assert.NotNull(userData);
        Assert.Equal(roleId, userData.Data.Roles.Single().RoleId);
        Assert.Equal(permissionCodes.OrderBy(p => p), userData.Data.Permissions.Select(p => p.PermissionCode).OrderBy(p => p));

        // Step 4: 更新用户角色
        permissionCodes = Permissions.AllPermissions.TakeLast(2).Select(p => p.Code).ToList();
        var newRoleId = await CreateTestRoleAsync("NewTestRole", "Description of the new test role", permissionCodes);

        var updateResponse = await _client.PutAsNewtonsoftJsonAsync($"/api/adminUser/UpdateAdminUserRoles/{testUserId}", new List<RoleId> { newRoleId });
        updateResponse.EnsureSuccessStatusCode();

        // Step 5: 验证用户更新后的角色和权限
        getUserDataResponse = await _client.GetAsync($"/api/AdminUser/GetAdminUserById/{testUserId}");
        getUserDataResponse.EnsureSuccessStatusCode();
        userData = await getUserDataResponse.Content.ReadFromNewtonsoftJsonAsync<ResponseData<AdminUserResponse>>();
        Assert.NotNull(userData);
        Assert.Equal(newRoleId, userData.Data.Roles.Single().RoleId);
        Assert.Equal(permissionCodes.OrderBy(p => p), userData.Data.Permissions.Select(p => p.PermissionCode).OrderBy(p => p));// 验证权限
    }

    /// <summary>
    /// 查询角色测试
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task CreateRole_And_QueryAdminUserByVariousMethods_ShouldSucceed()
    {
        await CreateTestRoleAsync("defaultRole", "defaultRole", []);

        var testRoleName = "TestQueryRole";
        var testRoleId = await CreateTestRoleAsync(testRoleName, "Test role", []);

        var allRolesResponse = await _client.GetAsync("/api/Role/GetAllRoles");
        allRolesResponse.EnsureSuccessStatusCode(); // 确保返回 200 OK

        var allRolesData = await allRolesResponse.Content.ReadFromNewtonsoftJsonAsync<ResponseData<IEnumerable<RoleResponse>>>();
        Assert.NotNull(allRolesData);
        Assert.NotEmpty(allRolesData.Data); // 验证返回的角色列表不为空

        var conditionResponse = await _client.GetAsync($"/api/Role/GetRolesByCondition?Name={testRoleName}");
        conditionResponse.EnsureSuccessStatusCode(); // 确保返回 200 OK
        var conditionData = await conditionResponse.Content.ReadFromNewtonsoftJsonAsync<ResponseData<IEnumerable<RoleResponse>>>();
        Assert.NotNull(conditionData);
        Assert.All(conditionData.Data, r => Assert.Contains(testRoleName, r.Name)); // 验证返回的用户符合条件

        var getRoleResponse = await _client.GetAsync($"/api/Role/GetRoleById/{testRoleId.Id}");
        getRoleResponse.EnsureSuccessStatusCode(); // 确保返回 200 OK
        var roleData = await getRoleResponse.Content.ReadFromNewtonsoftJsonAsync<ResponseData<RoleResponse>>();
        Assert.NotNull(roleData);
        Assert.Equal(testRoleId, roleData.Data.Id);
    }

    /// <summary>
    /// 更新角色权限测试
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task UpdateRolePermission_Test()
    {
        var permissionCodes = Permissions.AllPermissions.Take(2).Select(p => p.Code).ToList();
        var testRoleId = await CreateTestRoleAsync("TestUpdatePermissionRole", "", permissionCodes);
        var testUserId = await CreateTestAdminUser("TestUpdateRolePermissionsUser", [testRoleId]);

        var getRoleResponse = await _client.GetAsync($"api/Role/GetRoleById/{testRoleId}");
        getRoleResponse.EnsureSuccessStatusCode();
        var roleData = await getRoleResponse.Content.ReadFromNewtonsoftJsonAsync<ResponseData<RoleResponse>>();
        Assert.NotNull(roleData);
        Assert.Equal(permissionCodes.OrderBy(p => p), roleData.Data.Permissions.Select(p => p.PermissionCode).OrderBy(p => p));

        permissionCodes = Permissions.AllPermissions.TakeLast(2).Select(p => p.Code).ToList();
        var updateResponse = await _client.PutAsNewtonsoftJsonAsync($"/api/Role/UpdateRolePermissions/{testRoleId}", permissionCodes);
        updateResponse.EnsureSuccessStatusCode();

        getRoleResponse = await _client.GetAsync($"api/Role/GetRoleById/{testRoleId}");
        getRoleResponse.EnsureSuccessStatusCode();
        roleData = await getRoleResponse.Content.ReadFromNewtonsoftJsonAsync<ResponseData<RoleResponse>>();
        Assert.NotNull(roleData);
        Assert.Equal(permissionCodes.OrderBy(p => p), roleData.Data.Permissions.Select(p => p.PermissionCode).OrderBy(p => p));

        //验证关联用户的权限
        var getUserResponse = await _client.GetAsync($"/api/AdminUser/GetAdminUserById/{testUserId}");
        getUserResponse.EnsureSuccessStatusCode();
        var userData = await getUserResponse.Content.ReadFromNewtonsoftJsonAsync<ResponseData<AdminUserResponse>>();
        Assert.NotNull(userData);
        Assert.Equal(permissionCodes.OrderBy(p => p), userData.Data.Permissions.Select(p => p.PermissionCode).OrderBy(p => p));
    }
}