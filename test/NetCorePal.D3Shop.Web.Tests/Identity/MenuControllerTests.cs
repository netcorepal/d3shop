using NetCorePal.D3Shop.Admin.Shared.Permission;
using NetCorePal.D3Shop.Admin.Shared.Requests.MenuRequests;
using NetCorePal.D3Shop.Admin.Shared.Responses.MenuResponses;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;
using NetCorePal.Extensions.Dto;
using System.Net;
using System.Net.Http.Headers;

namespace NetCorePal.D3Shop.Web.Tests.Identity
{
    [Collection("web")]
    public class MenuControllerTests
    {
        private readonly HttpClient _client;

        public MenuControllerTests(MyWebApplicationFactory factory)
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

        #region CheckNameExists Tests

        [Fact]
        public async Task CheckNameExists_WithValidName_ShouldReturnFalse()
        {
            // Arrange
            var name = "TestMenu";

            // Act
            var response = await _client.GetAsync($"/api/system/Menu/name-exists?name={name}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadFromNewtonsoftJsonAsync<ResponseData<bool>>();
            Assert.NotNull(responseData);
            Assert.False(responseData.Data);
        }


        #endregion

        #region CheckPathExists Tests

        [Fact]
        public async Task CheckPathExists_WithValidPath_ShouldReturnFalse()
        {
            // Arrange
            var path = "/test/menu";

            // Act
            var response = await _client.GetAsync($"/api/system/Menu/path-exists?path={path}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadFromNewtonsoftJsonAsync<ResponseData<bool>>();
            Assert.NotNull(responseData);
            Assert.False(responseData.Data);
        }


        #endregion

        #region GetMenuList Tests

        [Fact]
        public async Task GetMenuList_ShouldReturnMenuList()
        {
            // Act
            var response = await _client.GetAsync("/api/system/Menu/list");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadFromNewtonsoftJsonAsync<ResponseData<List<MenuTreeNodeResponse>>>();
            Assert.NotNull(responseData);
            Assert.NotNull(responseData.Data);
        }

        #endregion

        #region CreateMenu Tests

        [Fact]
        public async Task CreateMenu_WithValidData_ShouldReturnMenuId()
        {
            // Arrange
            var request = new CreateMenuRequest
            {
                Name = "TestMenu",
                Path = "/test/menu",
                Type = MenuType.Menu,
                Pid = new MenuId(0),
                AuthCode = "test:menu",
                Component = "TestComponent",
                Redirect = "/test/redirect",
                Order = 1,
                Icon = "test-icon",
                Status = 0,
                Meta = new CreateMenuMetaRequest
                {
                    Title = "Test Menu",
                    Icon = "test-icon",
                    HideInBreadcrumb = false,
                    KeepAlive = true
                }
            };

            // Act
            var response = await _client.PostAsNewtonsoftJsonAsync("/api/system/Menu", request);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadFromNewtonsoftJsonAsync<ResponseData<MenuId>>();
            Assert.NotNull(responseData);
            Assert.NotEqual(0, responseData.Data.Id);
        }

        #endregion

        #region UpdateMenu Tests

        [Fact]
        public async Task UpdateMenu_WithValidData_ShouldReturnSuccess()
        {
            // Arrange
            // 1. 先创建一个菜单
            var createRequest = new CreateMenuRequest
            {
                Name = "TestMenuForUpdate",
                Path = "/test/menu/update",
                Type = MenuType.Menu,
                Pid = new MenuId(0),
                AuthCode = "test:menu:update",
                Component = "TestComponent",
                Redirect = "/test/redirect",
                Order = 1,
                Icon = "test-icon",
                Status = 0,
                Meta = new CreateMenuMetaRequest
                {
                    Title = "Test Menu For Update",
                    Icon = "test-icon",
                    HideInBreadcrumb = false,
                    KeepAlive = true
                }
            };

            var createResponse = await _client.PostAsNewtonsoftJsonAsync("/api/system/Menu", createRequest);
            createResponse.EnsureSuccessStatusCode();
            var createResponseData = await createResponse.Content.ReadFromNewtonsoftJsonAsync<ResponseData<MenuId>>();
            var menuId = createResponseData?.Data;

            // 2. 准备更新请求
            var updateRequest = new UpdateMenuRequest
            {
                Name = "UpdatedMenu",
                Path = "/updated/menu",
                Type = MenuType.Menu,
                Pid = new MenuId(0),
                AuthCode = "test:updated",
                Component = "UpdatedComponent",
                Redirect = "/updated/redirect",
                Order = 2,
                Status = 0,
                Meta = new UpdateMenuMetaRequest
                {
                    Title = "Updated Menu",
                    Icon = "updated-icon",
                    HideInBreadcrumb = false,
                    KeepAlive = true
                }
            };

            // Act
            var response = await _client.PutAsNewtonsoftJsonAsync($"/api/system/Menu/{menuId?.Id}", updateRequest);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadFromNewtonsoftJsonAsync<ResponseData<MenuId>>();
            Assert.NotNull(responseData);
            Assert.Equal(menuId?.Id, responseData.Data.Id);

            // 验证更新后的菜单数据
            var getResponse = await _client.GetAsync($"/api/system/Menu/{menuId?.Id}");
            getResponse.EnsureSuccessStatusCode();
            var getResponseData = await getResponse.Content.ReadFromNewtonsoftJsonAsync<ResponseData<MenuTreeNodeResponse>>();
            Assert.NotNull(getResponseData);
            Assert.Equal(updateRequest.Name, getResponseData.Data.Name);
            Assert.Equal(updateRequest.Path, getResponseData.Data.Path);
            Assert.Equal(updateRequest.Type.ToString().ToLower(), getResponseData.Data.Type);
            Assert.Equal(updateRequest.AuthCode, getResponseData.Data.AuthCode);
            Assert.Equal(updateRequest.Component, getResponseData.Data.Component);
            Assert.Equal(updateRequest.Redirect, getResponseData.Data.Redirect);
            Assert.Equal(updateRequest.Status, getResponseData.Data.Status);
        }

        

        #endregion

        #region DeleteMenu Tests

        [Fact]
        public async Task DeleteMenu_WithValidId_ShouldReturnSuccess()
        {
            // Arrange
            // 1. 先创建一个菜单
            var createRequest = new CreateMenuRequest
            {
                Name = "TestMenuForDelete",
                Path = "/test/menu/delete",
                Type = MenuType.Menu,
                Pid = new MenuId(0),
                AuthCode = "test:menu:delete",
                Component = "TestComponent",
                Redirect = "/test/redirect",
                Order = 1,
                Icon = "test-icon",
                Status = 0,
                Meta = new CreateMenuMetaRequest
                {
                    Title = "Test Menu For Delete",
                    Icon = "test-icon",
                    HideInBreadcrumb = false,
                    KeepAlive = true
                }
            };

            var createResponse = await _client.PostAsNewtonsoftJsonAsync("/api/system/Menu", createRequest);
            createResponse.EnsureSuccessStatusCode();
            var createResponseData = await createResponse.Content.ReadFromNewtonsoftJsonAsync<ResponseData<MenuId>>();

            // 验证创建响应
            Assert.NotNull(createResponseData);
            Assert.NotNull(createResponseData.Data);
            var menuId = createResponseData.Data;
            Assert.NotEqual(0, menuId.Id);

            // 验证菜单已创建
            var getResponse = await _client.GetAsync($"/api/system/Menu/{menuId.Id}");
            getResponse.EnsureSuccessStatusCode();
            var getResponseData = await getResponse.Content.ReadFromNewtonsoftJsonAsync<ResponseData<MenuTreeNodeResponse>>();
            Assert.NotNull(getResponseData);
            Assert.NotNull(getResponseData.Data);
            Assert.Equal(createRequest.Name, getResponseData.Data.Name);

            // Act
            var deleteResponse = await _client.DeleteAsync($"/api/system/Menu/{menuId.Id}");

            // Assert
            deleteResponse.EnsureSuccessStatusCode();
            var deleteResponseData = await deleteResponse.Content.ReadFromNewtonsoftJsonAsync<ResponseData<MenuId>>();
            Assert.NotNull(deleteResponseData);
            Assert.Equal(menuId.Id, deleteResponseData.Data.Id);

            // 验证菜单确实被删除
            var verifyResponse = await _client.GetAsync($"/api/system/Menu/{menuId.Id}");
            var verifyResponseData = await verifyResponse.Content.ReadFromNewtonsoftJsonAsync<ResponseData<MenuTreeNodeResponse>>();
            Assert.NotNull(verifyResponseData);
            Assert.Equal("菜单不存在", verifyResponseData.Message);

        }
       

        #endregion

        #region SetMenuVisibility Tests

        [Fact]
        public async Task SetMenuVisibility_WithValidData_ShouldReturnSuccess()
        {
            // Arrange
            var menuId = 1;
            var request = new SetVisibilityRequest { IsVisible = true };

            // Act
            var response = await _client.PutAsNewtonsoftJsonAsync($"/api/system/Menu/{menuId}/visibility", request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        #endregion

        #region SetMenuEnabled Tests

        [Fact]
        public async Task SetMenuEnabled_WithValidData_ShouldReturnSuccess()
        {
            // Arrange
            var menuId = 1;
            var request = new SetEnabledRequest { IsEnabled = true };

            // Act
            var response = await _client.PutAsNewtonsoftJsonAsync($"/api/system/Menu/{menuId}/enabled", request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        #endregion
    }
}