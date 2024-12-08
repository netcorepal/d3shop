using System.Net.Http.Headers;
using System.Net.Http.Json;
using NetCorePal.D3Shop.Admin.Shared.Requests;
using NetCorePal.D3Shop.Admin.Shared.Responses;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.Permission;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.Extensions.Dto;

namespace NetCorePal.D3Shop.Web.Tests.Identity;

[Collection("web")]
public class AdminUserRoleIntegrationTests
{
    private readonly HttpClient _client;

    public AdminUserRoleIntegrationTests(MyWebApplicationFactory factory)
    {
        _client = factory.WithWebHostBuilder(builder => { builder.ConfigureServices(_ => { }); })
            .CreateClient();
        const string json = $$"""
                              {
                                   "name": "{{AppDefaultCredentials.Name}}",
                                   "password": "{{AppDefaultCredentials.Password}}"
                              }
                              """;
        var content = new StringContent(json);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        _client.PostAsync("api/AdminUserAccount/login", content).GetAwaiter().GetResult();
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
        var createAdminUserRequest = new CreateAdminUserRequest
        {
            Name = name,
            PassWord = "123",
            Phone = "1",
            RoleIds = roleIds
        };

        // Act - 期望用户创建成功
        var response =
            await _client.PostAsNewtonsoftJsonAsync("/api/AdminUser/CreateAdminUser", createAdminUserRequest);

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
        const string testUserName = "TestQueryUser";
        await CreateTestAdminUser(testUserName, []);

        // Act 2: Get all admin users
        var allUsersResponse = await _client.GetAsync("/api/AdminUser/GetAllAdminUsers?PageIndex=1");
        allUsersResponse.EnsureSuccessStatusCode(); // 确保返回 200 OK

        var allUsersData = await allUsersResponse.Content
            .ReadFromNewtonsoftJsonAsync<ResponseData<PagedData<AdminUserResponse>>>();
        Assert.NotNull(allUsersData);
        Assert.NotEmpty(allUsersData.Data.Items); // 验证返回的用户列表不为空

        // Act 3: Get admin users by condition
        var conditionResponse =
            await _client.GetAsync($"/api/AdminUser/GetAllAdminUsers?Name={testUserName}&PageIndex=1");
        conditionResponse.EnsureSuccessStatusCode(); // 确保返回 200 OK
        var conditionData = await conditionResponse.Content
            .ReadFromNewtonsoftJsonAsync<ResponseData<PagedData<AdminUserResponse>>>();
        Assert.NotNull(conditionData);
        Assert.All(conditionData.Data.Items, user => Assert.Contains(testUserName, user.Name)); // 验证返回的用户符合条件
    }

    /// <summary>
    /// 创建测试角色
    /// </summary>
    /// <param name="roleName"></param>
    /// <param name="roleDescription"></param>
    /// <param name="permissionCodes"></param>
    /// <returns></returns>
    private async Task<RoleId> CreateTestRoleAsync(string roleName, string roleDescription,
        IEnumerable<string> permissionCodes)
    {
        // Arrange
        var createRoleRequest = new CreateRoleRequest
            { Name = roleName, Description = roleDescription, PermissionCodes = permissionCodes };

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
        var permissionCodes = PermissionDefinitionContext.AllPermissions
            .Take(2).Select(p => p.Code).ToList();
        var roleId = await CreateTestRoleAsync("TestRole", "Description of the test role", permissionCodes);

        // Step 2: 创建测试用户并分配角色
        const string testUserName = "TestUpdateRolesUsers";
        var testUserId = await CreateTestAdminUser(testUserName, [roleId]);

        // Step 3: 验证用户初始角色和权限
        var getAdminUserRolesResponse = await _client.GetAsync($"/api/AdminUser/GetAdminUserRoles/{testUserId}");
        getAdminUserRolesResponse.EnsureSuccessStatusCode();
        var adminUserRoles = (
            await getAdminUserRolesResponse.Content
                .ReadFromNewtonsoftJsonAsync<ResponseData<List<AdminUserRoleResponse>>>())?.Data;
        Assert.NotNull(adminUserRoles);
        Assert.Equal(roleId, adminUserRoles.Single(r => r.IsAssigned).RoleId);
        var getAdminUserPermissionsResponse =
            await _client.GetAsync($"/api/AdminUser/GetAssignedPermissions/{testUserId}");
        var adminUserPermissions =
            (await getAdminUserPermissionsResponse.Content
                .ReadFromNewtonsoftJsonAsync<ResponseData<List<AdminUserAssignedPermissionResponse>>>())?.Data;
        Assert.NotNull(adminUserPermissions);
        Assert.Equal(permissionCodes.OrderBy(p => p),
            adminUserPermissions.Select(p => p.Code).OrderBy(p => p));

        // Step 4: 更新用户角色
        permissionCodes = PermissionDefinitionContext.AllPermissions
            .TakeLast(2).Select(p => p.Code).ToList();
        var newRoleId = await CreateTestRoleAsync("NewTestRole", "Description of the new test role", permissionCodes);

        var updateResponse = await _client.PutAsNewtonsoftJsonAsync($"/api/adminUser/UpdateAdminUserRoles/{testUserId}",
            new List<RoleId> { newRoleId });
        updateResponse.EnsureSuccessStatusCode();

        // Step 5: 验证用户更新后的角色和权限
        getAdminUserRolesResponse = await _client.GetAsync($"/api/AdminUser/GetAdminUserRoles/{testUserId}");
        getAdminUserRolesResponse.EnsureSuccessStatusCode();
        adminUserRoles =
            (await getAdminUserRolesResponse.Content
                .ReadFromNewtonsoftJsonAsync<ResponseData<List<AdminUserRoleResponse>>>())?.Data;
        Assert.NotNull(adminUserRoles);
        Assert.Equal(newRoleId, adminUserRoles.Single(r => r.IsAssigned).RoleId);
        getAdminUserPermissionsResponse =
            await _client.GetAsync($"/api/AdminUser/GetAssignedPermissions/{testUserId}");
        adminUserPermissions =
            (await getAdminUserPermissionsResponse.Content
                .ReadFromNewtonsoftJsonAsync<ResponseData<List<AdminUserAssignedPermissionResponse>>>())?.Data;
        Assert.NotNull(adminUserPermissions);
        Assert.Equal(permissionCodes.OrderBy(p => p),
            adminUserPermissions.Select(p => p.Code).OrderBy(p => p));
    }

    /// <summary>
    /// 查询角色测试
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task CreateRole_And_QueryAdminUserByVariousMethods_ShouldSucceed()
    {
        await CreateTestRoleAsync("defaultRole", "defaultRole", []);

        const string testRoleName = "TestQueryRole";
        await CreateTestRoleAsync(testRoleName, "Test role", []);

        var allRolesResponse = await _client.GetAsync("/api/Role/GetAllRoles?PageIndex=1");
        allRolesResponse.EnsureSuccessStatusCode(); // 确保返回 200 OK

        var allRolesData = await allRolesResponse.Content
            .ReadFromNewtonsoftJsonAsync<ResponseData<PagedData<RoleResponse>>>();
        Assert.NotNull(allRolesData);
        Assert.NotEmpty(allRolesData.Data.Items); // 验证返回的角色列表不为空

        var conditionResponse = await _client.GetAsync($"/api/Role/GetAllRoles?Name={testRoleName}&PageIndex=1");
        conditionResponse.EnsureSuccessStatusCode(); // 确保返回 200 OK
        var conditionData = await conditionResponse.Content
            .ReadFromNewtonsoftJsonAsync<ResponseData<PagedData<RoleResponse>>>();
        Assert.NotNull(conditionData);
        Assert.All(conditionData.Data.Items, r => Assert.Contains(testRoleName, r.Name)); // 验证返回的用户符合条件
    }

    /// <summary>
    /// 更新角色权限测试
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task UpdateRolePermission_Test()
    {
        var permissionCodes = PermissionDefinitionContext.AllPermissions
            .Take(2).Select(p => p.Code).ToList();
        var testRoleId = await CreateTestRoleAsync("TestUpdatePermissionRole", "", permissionCodes);
        var testUserId = await CreateTestAdminUser("TestUpdateRolePermissionsUser", [testRoleId]);
        await CreateTestAdminUser("TestUpdateRolePermissionsUser2", [testRoleId]);

        var getRolePermissionsResponse = await _client.GetAsync($"api/Role/GetAssignedPermissionCodes/{testRoleId}");
        getRolePermissionsResponse.EnsureSuccessStatusCode();
        var rolePermissions = (await getRolePermissionsResponse.Content
            .ReadFromNewtonsoftJsonAsync<ResponseData<List<string>>>())?.Data;
        Assert.NotNull(rolePermissions);
        Assert.Equal(permissionCodes.OrderBy(p => p), rolePermissions.OrderBy(p => p));

        permissionCodes = PermissionDefinitionContext.AllPermissions
            .TakeLast(2).Select(p => p.Code).ToList();
        var updateResponse =
            await _client.PutAsNewtonsoftJsonAsync($"/api/Role/UpdateRolePermissions/{testRoleId}", permissionCodes);
        updateResponse.EnsureSuccessStatusCode();

        getRolePermissionsResponse = await _client.GetAsync($"api/Role/GetAssignedPermissionCodes/{testRoleId}");
        getRolePermissionsResponse.EnsureSuccessStatusCode();
        rolePermissions =
            (await getRolePermissionsResponse.Content
                .ReadFromNewtonsoftJsonAsync<ResponseData<List<string>>>())?.Data;
        Assert.NotNull(rolePermissions);
        Assert.Equal(permissionCodes.OrderBy(p => p), rolePermissions.OrderBy(p => p));

        //验证关联用户的权限
        var getAdminUserPermissionResponse =
            await _client.GetAsync($"/api/AdminUser/GetAssignedPermissions/{testUserId}");
        var adminUserPermissions =
            (await getAdminUserPermissionResponse.Content
                .ReadFromNewtonsoftJsonAsync<ResponseData<List<AdminUserAssignedPermissionResponse>>>())?.Data;
        Assert.NotNull(adminUserPermissions);
        Assert.Equal(permissionCodes.OrderBy(p => p),
            adminUserPermissions.Select(p => p.Code).OrderBy(p => p));
    }

    /// <summary>
    /// 设置用户权限测试
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task SetAdminUserSpecificPermissions_Test()
    {
        List<string> rolePermissionCodes = [PermissionCodes.RoleCreate, PermissionCodes.RoleDelete];
        var testRoleId = await CreateTestRoleAsync("TestSetAdminUserSpecificPermissionsRole", "", rolePermissionCodes);
        var testUserId = await CreateTestAdminUser("TestSetAdminUserSpecificPermissionsUser", [testRoleId]);

        var userPermissionsResponse = await _client.GetAsync($"/api/AdminUser/GetAssignedPermissions/{testUserId}");
        userPermissionsResponse.EnsureSuccessStatusCode(); // 确保返回 200 OK

        var userPermissions = (await userPermissionsResponse.Content
            .ReadFromNewtonsoftJsonAsync<ResponseData<List<AdminUserAssignedPermissionResponse>>>())?.Data;
        Assert.NotNull(userPermissions);
        Assert.True(userPermissions.All(p => rolePermissionCodes.Contains(p.Code) && p.IsFromRole));

        const string specificPermission = PermissionCodes.AdminUserSetPermissions;
        var updateResponse =
            await _client.PutAsJsonAsync<List<string>>($"/api/AdminUser/SetAdminUserSpecificPermissions/{testUserId}",
                [specificPermission]);
        updateResponse.EnsureSuccessStatusCode();

        userPermissionsResponse = await _client.GetAsync($"/api/AdminUser/GetAssignedPermissions/{testUserId}");
        userPermissionsResponse.EnsureSuccessStatusCode(); // 确保返回 200 OK
        userPermissions = (await userPermissionsResponse.Content
            .ReadFromNewtonsoftJsonAsync<ResponseData<List<AdminUserAssignedPermissionResponse>>>())?.Data;
        Assert.NotNull(userPermissions);
        var assignedPermissions = userPermissions.ToList();
        Assert.True(assignedPermissions.Count == 3);
        Assert.False(assignedPermissions.Single(p => p.Code == specificPermission).IsFromRole);
    }
}