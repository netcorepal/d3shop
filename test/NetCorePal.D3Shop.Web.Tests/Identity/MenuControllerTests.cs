using NetCorePal.D3Shop.Admin.Shared.Permission;
using NetCorePal.D3Shop.Admin.Shared.Requests.MenuRequests;
using NetCorePal.D3Shop.Admin.Shared.Responses.MenuResponses;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;
using NetCorePal.Extensions.Dto;
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

        [Fact]
        public async Task CheckNameExists_WithEmptyName_ShouldThrowException()
        {
            // Arrange
            var name = "";

            // Act
            var response = await _client.GetAsync($"/api/system/Menu/name-exists?name={name}");

            // Assert
            Assert.False(response.IsSuccessStatusCode);
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

        [Fact]
        public async Task CheckPathExists_WithEmptyPath_ShouldThrowException()
        {
            // Arrange
            var path = "";

            // Act
            var response = await _client.GetAsync($"/api/system/Menu/path-exists?path={path}");

            // Assert
            Assert.False(response.IsSuccessStatusCode);
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
            var menuId = 1;
            var request = new UpdateMenuRequest
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
            var response = await _client.PutAsNewtonsoftJsonAsync($"/api/system/Menu/{menuId}", request);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadFromNewtonsoftJsonAsync<ResponseData<MenuId>>();
            Assert.NotNull(responseData);
            Assert.Equal(menuId, responseData.Data.Id);
        }

        #endregion

        #region DeleteMenu Tests

        [Fact]
        public async Task DeleteMenu_WithValidId_ShouldReturnSuccess()
        {
            // Arrange
            var menuId = 1;

            // Act
            var response = await _client.DeleteAsync($"/api/system/Menu/{menuId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadFromNewtonsoftJsonAsync<ResponseData<MenuId>>();
            Assert.NotNull(responseData);
            Assert.Equal(menuId, responseData.Data.Id);
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