using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace LinqExtensions
{
    public static class WhereExtensionMethods
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
           => condition ? source.Where(predicate) : source;

        public static IQueryable<T> WhereNotNull<T>(this IQueryable<T> source, object? value, Expression<Func<T, bool>> predicate)
        {
            Exceptions.ThrowIfNull(source, nameof(source));
            Exceptions.ThrowIfNull(predicate, nameof(predicate));
            
            if (value is string && !string.IsNullOrWhiteSpace((string)value))
                return source.Where(predicate);
            else if (value != null)
                return source.Where(predicate);
            else
                return source;
        }

        public static IQueryable<T> WhereAny<T>(this IQueryable<T> source, bool[] conditions, Expression<Func<T, int, bool>> predicate)
        {
            Exceptions.ThrowIfNull(source, nameof(source));
            var conds = conditions.ToList();
            return conds.Any() ? source.Where(predicate) : source;
        }

        public static IQueryable<T> WhereBulk<T>(this IQueryable<T> source, bool condition, params Func<IQueryable<T>, IQueryable<T>>[] queries)
        {
            Exceptions.ThrowIfNull(source, nameof(source));
            if (condition)
                queries.ToList().ForEach(a => source = a(source));
            return source;
        }

        public static IQueryable<T> MatchIntsByConvention<T, D>(this IQueryable<T> source, D Filter, bool ignoreNulls = false)
        {
            return MatchPropsByTypeAndConvention(source, Filter, typeof(int),ignoreNulls);
        }
        public static IQueryable<T> MatchGuidsByConvention<T, D>(this IQueryable<T> source, D Filter, bool ignoreNulls = false)
        {
            return MatchPropsByTypeAndConvention(source, Filter, typeof(Guid), ignoreNulls);
        }

        private static IQueryable<T> MatchPropsByTypeAndConvention<T, D>(IQueryable<T> source, D Filter, Type type, bool ignoreNulls)
        {
            Exceptions.ThrowIfNull(source, nameof(source));
            Exceptions.ThrowIfNull(Filter, nameof(Filter));
            Exceptions.ThrowIfNull(type, nameof(type));


            var tType = typeof(T);
            var props = tType.GetProperties();
            var intProps = props.Where(a => a.PropertyType == type).ToList();

            var dType = typeof(D);
            var dProps = dType.GetProperties();
            var dIntProps = dProps.Where(a => a.PropertyType == type).ToList();

            var dIntPropNames = dIntProps.Select(a => a.Name).ToList();
            var tIntProps = intProps.Where(a => dIntPropNames.Contains(a.Name)).ToList();

            tIntProps.ForEach(x =>
            {
                var value = (int?)dType.GetProperty(x.Name).GetValue(Filter);
                if (value == null && ignoreNulls) return;
                source = source.Where(a => EF.Property<int>(a, x.Name) == value);
            });

            return source;
        }
    }
}