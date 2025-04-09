using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;

namespace NetCorePal.D3Shop.Web.Controllers.Identity.VueAdmin.Responses
{
    /// <summary>
    /// 菜单数据
    /// </summary>
    public class MenuResponse
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public MenuId Id { get; set; } = default!;
        /// <summary>
        /// 父菜单ID
        /// </summary>
        public MenuId? Pid { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// 菜单路径
        /// </summary>
        public string Path { get; set; } = string.Empty;
        /// <summary>
        /// 组件路径
        /// </summary>
        public string? Component { get; set; }
        /// <summary>
        /// 重定向路径
        /// </summary>
        public string? Redirect { get; set; }
        /// <summary>
        /// 菜单类型
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 菜单图标
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// 权限代码
        /// </summary>
        public string? AuthCode { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 菜单元数据
        /// </summary>
        public MenuMeta Meta { get; set; } = new();

        public IEnumerable<MenuResponse> Children { get; set; } = new List<MenuResponse>();
    }
}
