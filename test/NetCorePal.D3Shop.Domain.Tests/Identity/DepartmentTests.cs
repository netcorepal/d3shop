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

        // 测试：添加新用户
        [Fact]
        public void UpdateDepartInfo_ShouldAddNewUsers()
        {
            // Act：调用 UpdateDepartInfo 方法更新用户列表
            _department.UpdateDepartInfo(_department.Name, "001", _department.Description, 0);
                
            // Assert：验证新用户是否已成功添加
            Assert.Contains(_department.Users, u => u.UserId.Equals(new AdminUserId(3)) && u.UserName == "User3");
        }

        // 测试：移除已不存在的用户
        [Fact]
        public void UpdateDepartInfo_ShouldRemoveAbsentUsers()
        {
   
            // Act：调用 UpdateDepartInfo 方法更新用户列表
            _department.UpdateDepartInfo(_department.Name,"001", _department.Description,0);

            // Assert：验证 User1 是否被移除
            Assert.DoesNotContain(_department.Users, u => u.UserId.Equals(new AdminUserId(1)) && u.UserName == "User1");
        }

        // 测试：保留没有改变的用户
        [Fact]
        public void UpdateDepartInfo_ShouldRetainUnchangedUsers()
        {
            // Act：调用 UpdateDepartInfo 方法更新用户列表
            _department.UpdateDepartInfo(_department.Name,"001", _department.Description,0);

            // Assert：验证用户列表中依然只有 2 个用户，没有丢失或重复
            Assert.Equal(2, _department.Users.Count); // 确保用户数量没有变化
        }
    }




}

     

