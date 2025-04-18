using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;

namespace NetCorePal.D3Shop.Domain.Tests.Identity
{


    public class DepartmentTests
    {
        private readonly Department _department;

        // 构造函数：初始化 Department 实例，使用构造函数传入初始数据
        public DepartmentTests()
        {
            

            // 使用构造函数创建 Department 实例，传入部门名称、描述、父部门 ID 和用户列表
            _department = new Department("InitialName", "InitialDescription", new DeptId(1), 0);
        }

        // 测试：更新部门名称和描述
        [Fact]
        public void UpdateDepartInfo_ShouldUpdateNameAndDescription()
        {
            // Arrange：准备更新后的名称和描述
            var newName = "UpdatedName";
            var newDescription = "UpdatedDescription";

            // Act：调用 UpdateDepartInfo 方法更新部门名称和描述
            _department.UpdateDepartInfo(newName,"001", newDescription,0);

            // Assert：验证名称和描述是否正确更新
            Assert.Equal(newName, _department.Name);
            Assert.Equal(newDescription, _department.Description);
        }

   

     

    }




}

     

