using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePal.D3Shop.Admin.Shared.Requests.MenuRequests
{
    /// <summary>
    /// 创建菜单请求
    /// </summary>
    public class CreateMenuRequest
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// 菜单路径
        /// </summary>
        public string Path { get; set; } = string.Empty;
        /// <summary>
        /// 父菜单ID
        /// </summary>
        public MenuId Pid { get; set; } = new MenuId(0);
        /// <summary>
        /// 菜单类型
        /// </summary>
        public MenuType Type { get; set; }
        /// <summary>
        /// 权限代码
        /// </summary>
        public string AuthCode { get; set; } = string.Empty;
        /// <summary>
        /// 组件路径
        /// </summary>
        public string Component { get; set; } = string.Empty;
        /// <summary>
        /// 重定向路径
        /// </summary>
        public string Redirect { get; set; } = string.Empty;
        /// <summary>
        /// 排序顺序
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// 菜单图标
        /// </summary>
        public string Icon { get; set; } = string.Empty;

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        public CreateMenuMetaRequest Meta { get; set; } = new CreateMenuMetaRequest();
    }
}
