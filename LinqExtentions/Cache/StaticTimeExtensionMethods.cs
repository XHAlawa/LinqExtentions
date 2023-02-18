using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqExtensions.Cache
{
    public static class CacheableExtensionMethods
    {
        public static T Cache<T>(this IQueryable<T> query, Func<IQueryable<T>, T> dataCreator, string cacheName, int durationInMin = 30)
        {
            var obj = DI.MemoryCache.Get<Models.CacheModel<T>>(cacheName);

            Func<Models.CacheModel<T>?> CreateCache = () =>
            {
                return DI.MemoryCache.Set(cacheName, new Models.CacheModel<T>()
                {
                    WillExpiredOn = DateTime.Now.AddMinutes(durationInMin),
                    Data = dataCreator(query)
                });
            };

            if (obj == null)
            {
                obj = CreateCache();
            }
            else
            {
                if (obj.WillExpiredOn < DateTime.Now)
                {
                    DI.MemoryCache.Remove(cacheName);
                    obj = CreateCache();
                }
            }
            return obj.Data;
        }

        public static async Task<T> CacheAsync<T>(this IQueryable<T> query, Func<IQueryable<T>, Task<T>> dataCreator, string cacheName, int durationInMin = 30)
        {
            return query.Cache(a => dataCreator(query).Result, cacheName, durationInMin);
        }


        /// First
        public static T CacheFirst<T>(this IQueryable<T> query, string cacheName, int durationInMin = 30) =>
            query.Cache(a => a.First(), cacheName, durationInMin);
        public static T CacheFirstOrDefault<T>(this IQueryable<T> query, string cacheName, int durationInMin = 30) =>
            query.Cache(a => a.FirstOrDefault(), cacheName, durationInMin);

        public static async Task<T> CacheFirstAsync<T>(this IQueryable<T> query, string cacheName, int durationInMin = 30) =>
            await query.CacheAsync(async a => await a.FirstAsync(), cacheName, durationInMin);
        public static async Task<T> CacheFirstOrDefaultAsync<T>(this IQueryable<T> query, string cacheName, int durationInMin = 30) =>
            await query.CacheAsync(async a => await a.FirstOrDefaultAsync(), cacheName, durationInMin);


        ///  Last 
        public static T CacheLast<T>(this IQueryable<T> query, string cacheName, int durationInMin = 30) =>
            query.Cache(a => a.Last(), cacheName, durationInMin);
        public static T CacheLastOrDefault<T>(this IQueryable<T> query, string cacheName, int durationInMin = 30) =>
            query.Cache(a => a.LastOrDefault(), cacheName, durationInMin);

        
        public static async Task<T> CacheLastAsync<T>(this IQueryable<T> query, string cacheName, int durationInMin = 30) =>
            await query.CacheAsync(async a => await a.LastAsync(), cacheName, durationInMin);
        public static async Task<T> CacheLastOrDefaultAsync<T>(this IQueryable<T> query, string cacheName, int durationInMin = 30) =>
           await query.CacheAsync(async a => await a.LastOrDefaultAsync(), cacheName, durationInMin);

        /// Single
        public static T CacheSingle<T>(this IQueryable<T> query, string cacheName, int durationInMin = 30) =>
           query.Cache(a => a.Last(), cacheName, durationInMin);
        public static T CacheSingleOrDefault<T>(this IQueryable<T> query, string cacheName, int durationInMin = 30) =>
            query.Cache(a => a.SingleOrDefault(), cacheName, durationInMin);

        public static async Task<T> CacheSingleAsync<T>(this IQueryable<T> query, string cacheName, int durationInMin = 30) =>
           await query.CacheAsync(async a => await a.SingleAsync(), cacheName, durationInMin);
        public static async Task<T> CacheSingleOrDefaultAsync<T>(this IQueryable<T> query, string cacheName, int durationInMin = 30) =>
            await query.CacheAsync(async a => await a.SingleOrDefaultAsync(), cacheName, durationInMin);


        public static List<T> CacheList<T>(this IQueryable<T> query, Func<IQueryable<T>, List<T>> dataCreator, 
            string cacheName, int durationInMin = 30)
        {
            var obj = DI.MemoryCache.Get<Models.ListCacheModel<T>>(cacheName);

            Func<Models.ListCacheModel<T>?> CreateCache = () =>
            {
                return DI.MemoryCache.Set(cacheName, new Models.ListCacheModel<T>()
                {
                    WillExpiredOn = DateTime.Now.AddMinutes(durationInMin),
                    Data = dataCreator(query)
                });
            };

            if (obj == null)
            {
                obj = CreateCache();
            }
            else
            {
                if (obj.WillExpiredOn < DateTime.Now)
                {
                    DI.MemoryCache.Remove(cacheName);
                    obj = CreateCache();
                }
            }
            return obj.Data;
        }

        public static async Task<List<T>> CacheListAsync<T>(this IQueryable<T> query, Func<IQueryable<T>, Task<List<T>>> dataCreator,
            string cacheName, int durationInMin = 30)
        {
            return query.CacheList(a => dataCreator(query).Result, cacheName, durationInMin);
        }

        /// List
        public static List<T> CacheToList<T>(this IQueryable<T> query, string cacheName, int durationInMin = 30) =>
           query.CacheList(a => a.ToList(), cacheName, durationInMin);
        public static async Task<List<T>> CacheToListAsync<T>(this IQueryable<T> query, string cacheName, int durationInMin = 30) =>
            await query.CacheListAsync(a => a.ToListAsync(), cacheName, durationInMin);
    }
}
