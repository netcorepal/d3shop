using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePal.D3Shop.Domain.Tests.Identity
{


    public class DepartmentTests
    {
        private readonly Department _department;

        // 构造函数：初始化 Department 实例，使用构造函数传入初始数据
        public DepartmentTests()
        {
            // 创建初始的用户列表
            var initialUsers = new List<DepartmentUser>
        {
            new DepartmentUser("User1", new AdminUserId(1)), // 创建 User1
            new DepartmentUser("User2", new AdminUserId(2))  // 创建 User2
        };

            // 使用构造函数创建 Department 实例，传入部门名称、描述、父部门 ID 和用户列表
            _department = new Department("InitialName", "InitialDescription", new DeptId(1), initialUsers);
        }

        // 测试：更新部门名称和描述
        [Fact]
        public void UpdateDepartInfo_ShouldUpdateNameAndDescription()
        {
            // Arrange：准备更新后的名称和描述
            var newName = "UpdatedName";
            var newDescription = "UpdatedDescription";

            // Act：调用 UpdateDepartInfo 方法更新部门名称和描述
            _department.UpdateDepartInfo(newName, newDescription, _department.Users);

            // Assert：验证名称和描述是否正确更新
            Assert.Equal(newName, _department.Name);
            Assert.Equal(newDescription, _department.Description);
        }

        // 测试：添加新用户
        [Fact]
        public void UpdateDepartInfo_ShouldAddNewUsers()
        {
            // Arrange：准备新的用户列表，其中包括一个已有的用户和一个新用户
            var newUsers = new List<DepartmentUser>
        {
            new DepartmentUser("User1", new AdminUserId(1)),  // 已有用户 User1
            new DepartmentUser("User3", new AdminUserId(3))   // 新用户 User3
        };

            // Act：调用 UpdateDepartInfo 方法更新用户列表
            _department.UpdateDepartInfo(_department.Name, _department.Description, newUsers);

            // Assert：验证新用户是否已成功添加
            Assert.Contains(_department.Users, u => u.UserId.Equals(new AdminUserId(3)) && u.UserName == "User3");
        }

        // 测试：移除已不存在的用户
        [Fact]
        public void UpdateDepartInfo_ShouldRemoveAbsentUsers()
        {
            // Arrange：准备新的用户列表，其中仅包含 User2，User1 将被移除
            var newUsers = new List<DepartmentUser>
        {
            new DepartmentUser("User2", new AdminUserId(2)) // 仅包含 User2
            // User1 不在新列表中，应该被移除
        };

            // Act：调用 UpdateDepartInfo 方法更新用户列表
            _department.UpdateDepartInfo(_department.Name, _department.Description, newUsers);

            // Assert：验证 User1 是否被移除
            Assert.DoesNotContain(_department.Users, u => u.UserId.Equals(new AdminUserId(1)) && u.UserName == "User1");
        }

        // 测试：保留没有改变的用户
        [Fact]
        public void UpdateDepartInfo_ShouldRetainUnchangedUsers()
        {
            // Arrange：准备新的用户列表，User1 和 User2 都没有变化
            var newUsers = new List<DepartmentUser>
        {
            new DepartmentUser("User1", new AdminUserId(1)), // 保持不变的用户 User1
            new DepartmentUser("User2", new AdminUserId(2))  // 保持不变的用户 User2
        };

            // Act：调用 UpdateDepartInfo 方法更新用户列表
            _department.UpdateDepartInfo(_department.Name, _department.Description, newUsers);

            // Assert：验证用户列表中依然只有 2 个用户，没有丢失或重复
            Assert.Equal(2, _department.Users.Count); // 确保用户数量没有变化
        }
    }




}

     

