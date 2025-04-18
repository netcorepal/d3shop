using NetCorePal.D3Shop.Admin.Shared.Requests;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using System.Net.Http.Headers;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.D3Shop.Admin.Shared.Dtos.Identity;
using NetCorePal.Extensions.Dto;
using NetCorePal.D3Shop.Admin.Shared.Responses;

namespace NetCorePal.D3Shop.Web.Tests.Identity
{
    [Collection("web")]
    public class DepartmentTests
    {
        private readonly HttpClient _client;

        public DepartmentTests(MyWebApplicationFactory factory)
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

        #region CreateDepartment Tests

        [Fact]
        public async Task CreateDepartment_ShouldReturnDeptId()
        {
            // Arrange
            var request = new CreateDepartmentRequest
            {
                Name = "TestDepartment",
                Remark = "test decription",
                Pid = new DeptId(0)
                

            };

            // Act
            var response = await _client.PostAsNewtonsoftJsonAsync("/api/Department/CreateDepartment", request);

            // Assert - 创建成功
            Assert.True(response.IsSuccessStatusCode);
            var responseData = await response.Content.ReadFromNewtonsoftJsonAsync<ResponseData<DeptId>>();
            Assert.NotNull(responseData);
            Assert.NotEqual(0, responseData.Data.Id); // 验证返回的用户ID不是默认值
        }

        #endregion

        #region GetAllDepartments Tests

        [Fact]
        public async Task GetAllDepartments_ShouldReturnPagedData()
        {
            const string testDeptName = "TestDepartment";
            // Arrange
            var request = new DepartmentQueryRequest
            {
                //PageIndex = 1,
                //PageSize = 10,
                Name = testDeptName,
            };

            //var queryString = $"?PageIndex={request.PageIndex}&PageSize={request.PageSize}";
            var url = "/api/Department/GetAllDepartments";// + queryString;

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // 确保返回 200 OK
            var conditionData = await response.Content
                .ReadFromNewtonsoftJsonAsync<ResponseData<PagedData<DepartmentResponse>>>();
            Assert.NotNull(conditionData);
            Assert.All(conditionData.Data.Items, dept => Assert.Contains(testDeptName, dept.Name)); // 验证返回的用户符合条件
        }

        #endregion

        #region UpdateDepartmentInfo Tests

        [Fact]
        public async Task UpdateDepartmentInfo_ShouldReturnSuccess()
        {
            // Arrange
            var deptId = 1; // 假设部门 ID 为 1
            var request = new UpdateDepartmentInfoRequest
            {
                Name = "Updated Department",
                Remark = "Updated Description"
            };

            // Act
            var response = await _client.PutAsNewtonsoftJsonAsync($"/api/Department/UpdateDepartmentInfo/{deptId}", request);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsStringAsync();
            Assert.Contains("success", responseData, StringComparison.OrdinalIgnoreCase); 
        }

        #endregion

        #region DeleteDepartment Tests

        [Fact]
        public async Task DeleteDepartment_ShouldReturnSuccess()
        {
            // Arrange
            var deptId = 1; // 假设部门 ID 为 1

            // Act
            var response = await _client.DeleteAsync($"/api/Department/DeleteDepartment/{deptId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsStringAsync();
            Assert.Contains("success", responseData, StringComparison.OrdinalIgnoreCase); 
        }

        #endregion

    }



}
