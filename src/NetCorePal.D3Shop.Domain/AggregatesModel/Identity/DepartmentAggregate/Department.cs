using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.DomainEvents.Identity;
using NetCorePal.Extensions.Domain;
using NetCorePal.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public DeptId? ParentId { get; private set; } = default!;

        public DateTime CreatedAt { get; init; }

        public bool IsDeleted { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        public Department(string name, string description, DeptId? parentId)
        {
            Name = name;
            Description = description;
            ParentId = parentId;
        }

        /// <summary>
        /// 修改部门信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public void UpdateDepartInfo(string name, string description)
        {
            Name = name;
            Description = description;
            AddDomainEvent(new DepartmentInfoChangedDomainEvent(this));
        }

        public void Delete()
        {
            if (IsDeleted) throw new KnownException("部门已经被删除！");
            IsDeleted = true;
            DeletedAt = DateTime.Now;
        }
    }
}
