using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePal.D3Shop.Admin.Shared.Requests.MenuRequests
{
    public class UpdateMenuMetaRequest
    {

        /// <summary>
        /// 激活状态下的图标
        /// </summary>
        public string ActiveIcon { get; set; } = string.Empty;
        /// <summary>
        /// 激活状态下的路径
        /// </summary>
        public string ActivePath { get; set; } = string.Empty;

        /// <summary>
        /// 是否固定标签页
        /// </summary>
        public bool AffixTab { get; set; }
        /// <summary>
        /// 固定标签页的顺序
        /// </summary>
        public int AffixTabOrder { get; set; }
        /// <summary>
        /// 徽标内容
        /// </summary>
        public string Badge { get; set; } = string.Empty;
        /// <summary>
        /// 徽标类型
        /// </summary>
        public BadgeType? BadgeType { get; set; }
        /// <summary>
        /// 徽标颜色变体
        /// </summary>
        public BadgeVariant? BadgeVariants { get; set; }
        /// <summary>
        /// 是否在菜单中隐藏子项
        /// </summary>
        public bool HideChildrenInMenu { get; set; }
        /// <summary>
        /// 是否在面包屑中隐藏
        /// </summary>
        public bool HideInBreadcrumb { get; set; }
        /// <summary>
        /// 是否在菜单中隐藏
        /// </summary>
        public bool HideInMenu { get; set; }
        /// <summary>
        /// 是否在标签页中隐藏
        /// </summary>
        public bool HideInTab { get; set; }
        /// <summary>
        /// 菜单图标
        /// </summary>
        public string Icon { get; set; } = string.Empty;
        /// <summary>
        /// iframe源地址
        /// </summary>
        public string IframeSrc { get; set; } = string.Empty;
        /// <summary>
        /// 是否保持页面状态
        /// </summary>
        public bool KeepAlive { get; set; }
        /// <summary>
        /// 外部链接地址
        /// </summary>
        public string Link { get; set; } = string.Empty;
        /// <summary>
        /// 最大打开的标签页数量
        /// </summary>
        public int MaxNumOfOpenTab { get; set; }
        /// <summary>
        /// 是否不使用基础布局
        /// </summary>
        public bool NoBasicLayout { get; set; }
        /// <summary>
        /// 是否在新窗口打开
        /// </summary>
        public bool OpenInNewWindow { get; set; }
        /// <summary>
        /// 排序顺序
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// 查询参数
        /// </summary>
        public Dictionary<string, object> Query { get; set; } = new Dictionary<string, object>();
        /// <summary>
        /// 菜单标题
        /// </summary>
        public string Title { get; set; } = string.Empty;
    }
}
