using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LinqExtensions
{
    public static class ListExtensionMethods
    {
        public static T TakeTop<T>(this List<T> source)
        {
            Exceptions.ThrowIfNull(source, nameof(source));

            if (source.Count() == 0) return default(T);

            var topItem = source.First();
            source.RemoveAt(0);
            return topItem;
        }

        public static List<T> Flatten<T,TProp>(this List<T> source,Expression<Func<T, TProp>> expression)
        {
            Exceptions.ThrowIfNull(source, nameof(source));
            Exceptions.ThrowIfNull(expression, nameof(expression));

            var propNam = ((MemberExpression)expression.Body).Member.Name.ToLower();
            var props = typeof(T).GetProperties();
            var childProp = props.FirstOrDefault(p => p.Name.ToLower() == propNam);
            return FlattenList<T>(source, new List<T>(), childProp);
        }
        
        private static List<T> FlattenList<T>(List<T> source, List<T> newList, PropertyInfo? childProp)
        {
            source.ForEach(a =>
            {
                newList.Add(a);
                var childList = childProp.GetValue(a) as List<T>;
                if (childList != null)
                    newList = FlattenList<T>(childList, newList, childProp);
                childProp.SetValue(a, null);
            });
            return newList;
        }

        public static List<T> ToTree<T, TProp, SProp, CProp>(
            this List<T> source, 
            Expression<Func<T, TProp>> idProp, 
            Expression<Func<T, SProp>> parentIdProp,
            Expression<Func<T, CProp>> childListProp)
        {
            Exceptions.ThrowIfNull(source, nameof(source));
            Exceptions.ThrowIfNull(idProp, nameof(idProp));
            Exceptions.ThrowIfNull(parentIdProp, nameof(parentIdProp));
            Exceptions.ThrowIfNull(childListProp, nameof(childListProp));

            var idPropName = ((MemberExpression)idProp.Body).Member.Name.ToLower();
            var parentIdPropName = ((MemberExpression)parentIdProp.Body).Member.Name.ToLower();
            var childListPropName = ((MemberExpression)childListProp.Body).Member.Name.ToLower();
            
            
            var props = typeof(T).GetProperties();
            var idProperty = props.FirstOrDefault(p => p.Name.ToLower() == idPropName);
            var parentIdProperty = props.FirstOrDefault(p => p.Name.ToLower() == parentIdPropName);
            var childListProperty = props.FirstOrDefault(p => p.Name.ToLower() == childListPropName);
            var newList = new List<T>();
            newList = source.Where(x => parentIdProperty.GetValue(x) == null).ToList();

            ToTreeOperator(source, newList, idProperty, parentIdProperty, childListProperty);


            return newList;
        }

        private static void ToTreeOperator<T>(
            List<T> source, 
            List<T> childList, 
            PropertyInfo idProperty, 
            PropertyInfo parentIdProperty, 
            PropertyInfo childListProperty)
        {
            childList.ForEach(a =>
            {
                var childList = source.Where(X => parentIdProperty.GetValue(X) == idProperty.GetValue(a)).ToList();
                childListProperty.SetValue(a, childList);
                ToTreeOperator(source, childList, idProperty, parentIdProperty, childListProperty);
            });
        }
    }
}
