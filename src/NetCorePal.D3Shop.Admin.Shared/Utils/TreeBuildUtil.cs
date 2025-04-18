using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePal.D3Shop.Admin.Shared.Utils
{
    /// <summary>
    /// 树基类
    /// </summary>
    /// <typeparam name="TId">节点ID类型</typeparam>
    public interface ITreeNode<TId> where TId : class
    {
        /// <summary>
        /// 获取节点id
        /// </summary>
        /// <returns></returns>
        TId GetId();

        /// <summary>
        /// 获取节点父id
        /// </summary>
        /// <returns></returns>
        TId GetPid();

        /// <summary>
        /// 设置Children
        /// </summary>
        /// <param name="children"></param>
        void SetChildren(IList children);
    }

    /// <summary>
    /// 树构建扩展方法
    /// </summary>
    public static class TreeExtensions
    {
        /// <summary>
        /// 将集合转换为树结构
        /// </summary>
        /// <typeparam name="T">节点类型</typeparam>
        /// <typeparam name="TId">节点ID类型</typeparam>
        /// <param name="items">节点集合</param>
        /// <param name="idSelector">ID选择器</param>
        /// <param name="parentIdSelector">父ID选择器</param>
        /// <param name="childrenSetter">子节点设置器</param>
        /// <param name="rootParentId">根节点的父ID</param>
        /// <returns>树结构</returns>
        public static List<T> ToTree<T, TId>(
            this IEnumerable<T> items,
            Func<T, TId> idSelector,
            Func<T, TId> parentIdSelector,
            Func<T, IEnumerable<T>, T> childrenSetter,
            TId rootParentId)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            if (idSelector == null)
                throw new ArgumentNullException(nameof(idSelector));
            if (parentIdSelector == null)
                throw new ArgumentNullException(nameof(parentIdSelector));
            if (childrenSetter == null)
                throw new ArgumentNullException(nameof(childrenSetter));
            if (rootParentId == null)
                throw new ArgumentNullException(nameof(rootParentId));

            // 构建以父节点ID为键的查找表
            var lookup = items.ToLookup(parentIdSelector);
            
            // 获取根节点
            var roots = lookup[rootParentId].ToList();
            
            // 递归设置子节点
            foreach (var root in roots)
            {
                SetChildren(root, lookup, idSelector, childrenSetter);
            }
            
            return roots;
        }
        
        /// <summary>
        /// 递归设置子节点
        /// </summary>
        private static void SetChildren<T, TId>(
            T item,
            ILookup<TId, T> lookup,
            Func<T, TId> idSelector,
            Func<T, IEnumerable<T>, T> childrenSetter)
        {
            var children = lookup[idSelector(item)].ToList();
            childrenSetter(item, children);
            
            foreach (var child in children)
            {
                SetChildren(child, lookup, idSelector, childrenSetter);
            }
        }
        
        /// <summary>
        /// 将集合转换为树结构（适用于ITreeNode接口）
        /// </summary>
        /// <typeparam name="T">节点类型</typeparam>
        /// <typeparam name="TId">节点ID类型</typeparam>
        /// <param name="items">节点集合</param>
        /// <param name="rootParentId">根节点的父ID</param>
        /// <returns>树结构</returns>
        public static List<T> ToTree<T, TId>(
            this IEnumerable<T> items,
            TId rootParentId) where T : ITreeNode<TId> where TId : class
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            if (rootParentId == null)
                throw new ArgumentNullException(nameof(rootParentId));

            return items.ToTree(
                item => item.GetId(),
                item => item.GetPid(),
                (item, children) => { item.SetChildren(children.ToList()); return item; },
                rootParentId);
        }
    }
}
