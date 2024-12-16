using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate
{

    /// <summary>
    /// 部门用户
    /// </summary>
    public class DepartmentUser
    {
        /// <summary>
        /// 部门id
        /// </summary>
        public DeptId DeptId { get; set; } = default!;

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; private set; } = string.Empty;
    }
}
