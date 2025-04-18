using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.Extensions.Domain;
using NetCorePal.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate
{

    public partial record MenuId : IInt64StronglyTypedId;

    /// <summary>
    /// 系统菜单实体类，定义了菜单的基本结构和层级关系
    /// </summary>
    public class Menu : Entity<MenuId>, IAggregateRoot
    {

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; private set; } = string.Empty;
        /// <summary>
        /// 菜单路径
        /// </summary>
        public string Path { get; private set; } = string.Empty;

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 父菜单ID
        /// </summary>
        public MenuId ParentId { get; private set; } = new MenuId(0);
       

        /// <summary>
        /// 菜单类型
        /// </summary>
        public MenuType Type { get; private set; }
        /// <summary>
        /// 权限代码
        /// </summary>
        public string AuthCode { get; private set; } = string.Empty;
        /// <summary>
        /// 组件路径
        /// </summary>
        public string Component { get; private set; } = string.Empty;
        /// <summary>
        /// 重定向路径
        /// </summary>
        public string Redirect { get; private set; } = string.Empty;
        /// <summary>
        /// 排序顺序
        /// </summary>
        public int Order { get; private set; }
        /// <summary>
        /// 菜单图标
        /// </summary>
        public string Icon { get; private set; } = string.Empty;
        /// <summary>
        /// 是否可见
        /// </summary>
        public bool IsVisible { get; private set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; private set; }
        /// <summary>
        /// 菜单元数据
        /// </summary>
        public MenuMeta? Meta { get; private set; }


        /// <summary>
        /// 初始化菜单聚合根
        /// </summary>
        /// <param name="name">菜单名称</param>
        /// <param name="path">菜单路径</param>
        /// <param name="type">菜单类型</param>
        /// <param name="parentId">父菜单ID</param>
        /// <param name="authCode">权限代码</param>
        /// <param name="component">组件路径</param>
        /// <param name="redirect">重定向路径</param>
        /// <param name="order">排序顺序</param>
        /// <param name="icon">菜单图标</param>
        /// <param name="meta">菜单元数据</param>
        public Menu(string name, string path, MenuType type, MenuId parentId, string authCode, string component, string redirect, int order, string icon, int status, MenuMeta meta)
        {
            Name = name;
            Path = path;
            Type = type;
            ParentId = parentId;
            AuthCode = authCode;
            Component = component;
            Redirect = redirect;
            Order = order;
            Icon = icon;
            Meta = meta;
            Status = status;
            IsVisible = true;
            IsEnabled = true;
        }

        /// <summary>
        /// 更新菜单信息
        /// </summary>
        /// <param name="name">菜单名称</param>
        /// <param name="path">菜单路径</param>
        /// <param name="type">菜单类型</param>
        /// <param name="parentId">父菜单ID</param>
        /// <param name="authCode">权限代码</param>
        /// <param name="component">组件路径</param>
        /// <param name="redirect">重定向路径</param>
        /// <param name="order">排序顺序</param>
        /// <param name="icon">菜单图标</param>
        /// <param name="meta">菜单元数据</param>
        public void Update(string name, string path, MenuType type, MenuId parentId, string authCode, string component, string redirect, int order, string icon, int status, MenuMeta meta)
        {
            Name = name;
            Path = path;
            Type = type;
            ParentId = parentId;
            AuthCode = authCode;
            Component = component;
            Redirect = redirect;
            Order = order;
            Icon = icon;
            Status = status;
            Meta = meta;
        }

        /// <summary>
        /// 设置菜单可见性
        /// </summary>
        /// <param name="isVisible">是否可见</param>
        public void SetVisibility(bool isVisible)
        {
            IsVisible = isVisible;
        }

        /// <summary>
        /// 设置菜单启用状态
        /// </summary>
        /// <param name="isEnabled">是否启用</param>
        public void SetEnabled(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }




    }

    /// <summary>
    /// 菜单类型枚举，定义了系统中不同类型的菜单项
    /// </summary>
    public enum MenuType
    {
        /// <summary>
        /// 目录
        /// </summary>
        Catalog,
        /// <summary>
        /// 菜单
        /// </summary>
        Menu,
        /// <summary>
        /// 内嵌
        /// </summary>
        Embedded,
        /// <summary>
        /// 链接
        /// </summary>
        Link,
        /// <summary>
        /// 按钮
        /// </summary>
        Button
    }

    /// <summary>
    /// 徽标类型枚举，定义了菜单项上徽标的显示样式
    /// </summary>
    public enum BadgeType
    {
        /// <summary>
        /// 点状徽标，显示为一个小圆点
        /// </summary>
        Dot,
        /// <summary>
        /// 普通徽标，显示为文字或数字
        /// </summary>
        Normal
    }

    /// <summary>
    /// 徽标颜色枚举，定义了徽标的颜色变体
    /// </summary>
    public enum BadgeVariant
    {
        /// <summary>
        /// 默认颜色
        /// </summary>
        Default,
        /// <summary>
        /// 危险/错误颜色
        /// </summary>
        Destructive,
        /// <summary>
        /// 主要/强调颜色
        /// </summary>
        Primary,
        /// <summary>
        /// 成功颜色
        /// </summary>
        Success,
        /// <summary>
        /// 警告颜色
        /// </summary>
        Warning
    }
}
