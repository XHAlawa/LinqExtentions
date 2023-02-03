using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqExtentions
{
    public static class EnumrableExtentionMethods
    {
        public static void Each<T>(this IEnumerable<T> source, Action<object> action)
        {
            Exceptions.ThrowIfNull(source, nameof(source));
            Exceptions.ThrowIfNull(source, nameof(action));
        
            foreach (var item in source) action(item);
        }

        public static bool HasAtLeast<T>(this IEnumerable<T> source, T value, int count = 0)
        {
            Exceptions.ThrowIfNull(source, nameof(source));
            Exceptions.ThrowIfNull(source, nameof(value));
            Exceptions.ThrowIfNull(source, nameof(count));

            if (count == 0) return true;

            foreach(var item in source)
                if (item != null && item.Equals(value))
                {
                    count--;
                    if (count == 0) break;
                }
            
            return count == 0;
        }

        public static bool HasAtMost<T>(this IEnumerable<T> source, T value, int count = 0)
        {
            Exceptions.ThrowIfNull(source, nameof(source));
            Exceptions.ThrowIfNull(source, nameof(value));
            Exceptions.ThrowIfNull(source, nameof(count));

            if (count == 0) return true;

            int matchedCount = 0;

            foreach (var item in source)
                if (item != null && item.Equals(value))
                {
                    matchedCount++;
                    if (matchedCount > count) return false;
                }

            return matchedCount == count;
        }

        public static bool HasExactly<T>(this IEnumerable<T> source, T value, int count = 0)
        {
            Exceptions.ThrowIfNull(source, nameof(source));
            Exceptions.ThrowIfNull(source, nameof(value));
            Exceptions.ThrowIfNull(source, nameof(count));

            if (count == 0) return true;

            int matchedCount = 0;

            foreach (var item in source)
                if (item != null && item.Equals(value))
                    matchedCount++;

            return matchedCount == count;
        }

        public static T[] PickRandom<T>(this IEnumerable<T> source, int count = 0)
        {
            Exceptions.ThrowIfNull(count, nameof(source));
            Exceptions.ThrowIfNull(count, nameof(count));

            var s = source as T[] ?? source.ToArray();
            var list = new List<T>();
            var rand = new Random();

            for (int i = 0; i < count; i++)
                list.Add(s[rand.Next(s.Length)]);

            return list.ToArray();
        }

        public static T[] Repeate<T>(this IEnumerable<T> source, int count = 0)
        {
            Exceptions.ThrowIfNull(count, nameof(source));
            Exceptions.ThrowIfNull(count, nameof(count));

            var s = source as T[] ?? source.ToArray();
            var list = new List<T>();
            var rand = new Random();

            for (int i = 0; i < count; i++)
                list.AddRange(s);

            return list.ToArray();
        }

        public static T[] Exclude<T>(this IEnumerable<T> source, T value)
        {
            Exceptions.ThrowIfNull(source, nameof(source));
            Exceptions.ThrowIfNull(source, nameof(value));

            var list = new List<T>();

            foreach (var item in source)
                if (item != null && !item.Equals(value))
                    list.Add(item);

            return list.ToArray();
        }
    }
}
