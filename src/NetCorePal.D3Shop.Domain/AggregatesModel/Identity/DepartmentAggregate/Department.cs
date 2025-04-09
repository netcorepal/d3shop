using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.Extensions.Domain;
using NetCorePal.Extensions.Primitives;
using NetCorePal.D3Shop.Domain.DomainEvents.Identity.Admin;

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate
{
    public partial record DeptId : IInt64StronglyTypedId;

    /// <summary>
    /// 部门
    /// </summary>
    public class Department : Entity<DeptId>, IAggregateRoot
    {

        /// <summary>
        /// 部门名称
        /// </summary>
        public string Name { get; private set; } = string.Empty;

        /// <summary>
        /// 部门编码
        /// </summary>
        public string Code { get; private set; } = string.Empty;

        /// <summary>
        /// 是否启用
        /// </summary>
        public int Status { get; private set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; private set; } = string.Empty;

        /// <summary>
        /// 父部门id
        /// </summary>
        public DeptId ParentId { get; private set; } = new DeptId(0);

        public DateTime CreatedAt { get; init; }

        public bool IsDeleted { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        public virtual ICollection<DepartmentUser> Users { get; } = [];

        protected Department()
        {
        }

        public Department(string name, string description, DeptId parentId, IEnumerable<DepartmentUser> deptUsers,int status)
        {
            Name = name;
            Description = description;
            ParentId = parentId;
            CreatedAt = DateTime.Now;
            Status = status;
            foreach (var user in deptUsers)
            {
                Users.Add(user);
            }
        }

        /// <summary>
        /// 创建部门
        /// </summary>
        /// <param name="name">部门名称</param>
        /// <param name="code">部门编码</param>
        /// <param name="description">部门描述</param>
        /// <param name="parentId">父部门ID</param>
        /// <param name="isActive">是否启用</param>
        /// <param name="deptUsers">部门用户</param>
        /// <returns>部门实体</returns>
        //public static Department Create(
        //    string name,
        //    string code,
        //    string description,
        //    DeptId parentId,
        //    bool isActive,
        //    IEnumerable<DepartmentUser> deptUsers)
        //{
        //    var department = new Department(name, description, parentId, deptUsers)
        //    {
        //        Code = code,
        //        IsActive = isActive
        //    };

        //    return department;
        //}

        /// <summary>
        /// 修改部门信息
        /// </summary>
        /// <param name="name">部门名称</param>
        /// <param name="code">部门编码</param>
        /// <param name="description">部门描述</param>
        /// <param name="status">是否启用</param>
        /// <param name="deptUsers">部门用户</param>
        public void UpdateDepartInfo(
            string name,
            string code,
            string description,
            int status,
            IEnumerable<DepartmentUser> deptUsers)
        {
            Name = name;
            Code = code;
            Description = description;
            Status = status;

            var currentUserMap = Users.ToDictionary(r => r.UserId);
            var targetUserMap = deptUsers.ToDictionary(r => r.UserId);

            var userIdsToRemove = currentUserMap.Keys.Except(targetUserMap.Keys);
            foreach (var userId in userIdsToRemove)
            {
                Users.Remove(currentUserMap[userId]);
            }

            var userIdsToAdd = targetUserMap.Keys.Except(currentUserMap.Keys);
            foreach (var userId in userIdsToAdd)
            {
                var targetUser = targetUserMap[userId];
                Users.Add(targetUser);
            }

            AddDomainEvent(new DepartmentInfoChangedDomainEvent(this));
        }

        /// <summary>
        /// 更新部门状态
        /// </summary>
        /// <param name="status">是否启用</param>
        public void UpdateStatus(int status)
        {
            Status = status;
            AddDomainEvent(new DepartmentInfoChangedDomainEvent(this));
        }

        /// <summary>
        /// 更新部门用户名称
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="userName">用户名称</param>
        public void UpdateDepartmentUserName(AdminUserId userId, string userName)
        {
            var savedUser = Users.FirstOrDefault(r => r.UserId == userId);
            savedUser?.UpdateUserInfo(userName);
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        public void Delete()
        {
            if (IsDeleted) throw new KnownException("部门已经被删除！");
            IsDeleted = true;
            DeletedAt = DateTime.Now;
        }
    }
}
