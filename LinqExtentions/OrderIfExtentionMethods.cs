using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LinqExtentions
{
    public static class OrderIfExtentionMethods
    {
        public static IOrderedQueryable<TSource> OrderByIf<TSource, TKey>(this IQueryable<TSource> query, bool condition, Expression<Func<TSource, TKey>> orderByExpression)
        =>  condition ? query.OrderBy(orderByExpression) : (IOrderedQueryable<TSource>)query;

        public static IOrderedQueryable<TSource> OrderByDescendingIf<TSource, TKey>(this IQueryable<TSource> query, bool condition, Expression<Func<TSource, TKey>> orderByExpression)
        =>  condition ? query.OrderByDescending(orderByExpression) : (IOrderedQueryable<TSource>)query;

        public static IOrderedQueryable<TSource> ThenByIf<TSource, TKey>(this IOrderedQueryable<TSource> query, bool condition, Expression<Func<TSource, TKey>> orderByExpression)
        =>  condition ? query.ThenBy(orderByExpression) : query;

        public static IOrderedQueryable<TSource> ThenByDescendingIf<TSource, TKey>(this IOrderedQueryable<TSource> query, bool condition, Expression<Func<TSource, TKey>> orderByExpression)
        => condition ? query.ThenByDescending(orderByExpression) : query;
    }
}
