using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqExtensions
{
    public static class ConvertExtensionMethods
    {
        public static void SetNullIfEmpty(this object target, Type targetType)
        {
            Exceptions.ThrowIfNull(target, nameof(target));
            Exceptions.ThrowIfNull(targetType, nameof(targetType));

            if (targetType.IsValueType) return;

            if (targetType == typeof(string))
            {
                if (string.IsNullOrEmpty((string)target)) target = null;
                return;
            }

            if (targetType.IsArray)
            {
                if (((Array)target).Length == 0) target = null;
                return;
            }

            if (targetType.IsGenericType)
            {
                var genericType = targetType.GetGenericTypeDefinition();
                if (genericType == typeof(List<>) || genericType == typeof(HashSet<>))
                {
                    if (((IEnumerable<object>)target).Count() == 0) target = null;
                    return;
                }
            }

            if (targetType.IsClass)
            {
                if (targetType.GetProperties().All(p => p.GetValue(target) == null)) target = null;
                return;
            }
        }
    }
}
