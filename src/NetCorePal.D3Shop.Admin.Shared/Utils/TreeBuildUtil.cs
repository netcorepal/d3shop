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
    /// 树构建工具类
    /// </summary>
    /// <typeparam name="T">节点类型</typeparam>
    /// <typeparam name="TId">节点ID类型</typeparam>
    public class TreeBuildUtil<T, TId> where T : ITreeNode<TId> where TId : class
    {
        /// <summary>
        /// 顶级节点的父节点Id
        /// </summary>
        private TId _rootParentId;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="rootParentId">根节点的父ID</param>
        public TreeBuildUtil(TId rootParentId)
        {
            _rootParentId = rootParentId ?? throw new ArgumentNullException(nameof(rootParentId));
        }

        /// <summary>
        /// 设置根节点方法
        /// 查询数据可以设置其他节点为根节点，避免父节点永远是默认值，查询不到数据的问题
        /// </summary>
        public void SetRootParentId(TId rootParentId)
        {
            _rootParentId = rootParentId ?? throw new ArgumentNullException(nameof(rootParentId));
        }

        public List<T> Build(List<T> nodes)
        {
            if (nodes == null)
                throw new ArgumentNullException(nameof(nodes));
                
            // 构建以父节点ID为键的字典
            var nodeLookup = nodes.GroupBy(node => node.GetPid())
                                  .ToDictionary(group => group.Key, group => group.ToList());

            var result = new List<T>();
            nodes.ForEach(node =>
            {
                if (_rootParentId.Equals(node.GetPid()))
                {
                    result.Add(node);
                }
                if (nodeLookup.ContainsKey(node.GetId()))
                {
                    node.SetChildren(nodeLookup[node.GetId()]);
                }
            });

            return result;
        }
    }

  
}
