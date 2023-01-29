using System.Linq.Expressions;

namespace LinqExtentions
{
    public static class WhereExtentionMethods
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
           => condition ? source.Where(predicate) : source;

        public static IQueryable<T> WhereNotNull<T>(this IQueryable<T> source, object? value, Expression<Func<T, bool>> predicate)
        {
            if (value is string && !string.IsNullOrWhiteSpace((string)value))
                return source.Where(predicate);
            else if (value != null)
                return source.Where(predicate);
            else
                return source;
        }

        public static IQueryable<T> WhereAny<T>(this IQueryable<T> source, bool[] conditions, Expression<Func<T, int, bool>> predicate)
        {
            var conds = conditions.ToList();
            return conds.Any() ? source.Where(predicate) : source;
        }

        public static IQueryable<T> WhereBulk<T>(this IQueryable<T> source, bool condition, params Func<IQueryable<T>, IQueryable<T>>[] querys)
        {
            if (condition)
                querys.ToList().ForEach(a => source = a(source));
            return source;
        }
    }
}