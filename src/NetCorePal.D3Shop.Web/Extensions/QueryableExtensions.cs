using System.Linq.Expressions;

namespace NetCorePal.D3Shop.Web.Extensions;

/// <summary>
/// 一些针对 <see cref="IQueryable{T}"/> 的实用扩展方法。
/// </summary>
public static class QueryableExtensions
{
    
    /*/// <summary>
    /// 根据指定的条件，在 <see cref="IQueryable{T}"/> 上应用筛选操作。
    /// </summary>
    /// <param name="query">要应用筛选的 IQueryable</param>
    /// <param name="condition">一个布尔值，决定是否应用筛选</param>
    /// <param name="predicate">用于筛选的条件表达式</param>
    /// <returns>根据 <paramref name="condition"/> 返回已筛选或未筛选的查询</returns>
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition,
        Expression<Func<T, bool>> predicate)
    {
        return condition
            ? query.Where(predicate)
            : query;
    }*/

    /// <summary>
    /// 根据指定的条件，在 <see cref="IQueryable{T}"/> 上应用筛选操作。
    /// </summary>
    /// <param name="query">要应用筛选的 IQueryable</param>
    /// <param name="condition">一个布尔值，决定是否应用筛选</param>
    /// <param name="predicate">用于筛选的条件表达式</param>
    /// <returns>根据 <paramref name="condition"/> 返回已筛选或未筛选的查询</returns>
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition,
        Expression<Func<T, int, bool>> predicate)
    {
        return condition
            ? query.Where(predicate)
            : query;
    }
}