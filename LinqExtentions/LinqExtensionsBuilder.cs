using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqExtensions
{
    public static class LinqExtensionsBuilder
    {
        private static IServiceCollection ServiceCollection;
        internal static IServiceProvider ServiceProvider;
        public static void AddLinqExtensions(this IServiceCollection services)
        {
            services.AddMemoryCache();
            ServiceCollection = services;
            ServiceProvider = services.BuildServiceProvider();
        }
    }

   internal static class DI
   {
        private static IMemoryCache _memoryCahe;
        internal static IMemoryCache MemoryCache
        {
            get
            {
                if (_memoryCahe == null) _memoryCahe = GetService<IMemoryCache>();
                return _memoryCahe;
            }
        }
        public static T GetService<T>()
        {
            return LinqExtensionsBuilder.ServiceProvider.GetService<T>();
        }
   }
}
