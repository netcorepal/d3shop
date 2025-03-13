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

        public Department(string name, string description, DeptId parentId, IEnumerable<DepartmentUser> deptUsers)
        {
            Name = name;
            Description = description;
            ParentId = parentId;
            CreatedAt = DateTime.Now;
            foreach (var user in deptUsers)
            {
                Users.Add(user);
            }
        }

        /// <summary>
        /// 修改部门信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="deptUsers"></param>
        public void UpdateDepartInfo(string name, string description, IEnumerable<DepartmentUser> deptUsers)
        {
            Name = name;
            Description = description;

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


        public void UpdateDepartmentUserName(AdminUserId userId, string userName)
        {
            var savedUser = Users.FirstOrDefault(r => r.UserId == userId);
            savedUser?.UpdateUserInfo(userName);
        }


        public void Delete()
        {
            if (IsDeleted) throw new KnownException("部门已经被删除！");
            IsDeleted = true;
            DeletedAt = DateTime.Now;
        }
    }
}
