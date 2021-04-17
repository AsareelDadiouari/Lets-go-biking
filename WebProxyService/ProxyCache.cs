using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace WebProxyService
{
    public class ProxyCache<T> : ICache<T> where T : new()
    {
        public DateTimeOffset dt_default;
        ObjectCache cache;

        public ProxyCache() {
            dt_default = ObjectCache.InfiniteAbsoluteExpiration;
            cache = MemoryCache.Default;
        }

        public void Add(string CacheItem, T data)
        {
            cache.Set(CacheItem, data, dt_default);
        }

        public T Get(string CacheItem)
        {
            T item = (T)cache.Get(CacheItem);
            if (item == null)
            {
                var element = Activator.CreateInstance(typeof(T));
                cache.Add(CacheItem, new T(), dt_default);
            }

            return (T)cache[CacheItem];
        }

        public T Get(string CacheItem, DateTimeOffset dt)
        {
            T item = (T)cache.Get(CacheItem);

            if (item == null)
            {
                cache.Add(CacheItem, new T(), dt);
            }
            return (T)cache[CacheItem];
        }

        public T Get(string CacheItem, double dt_seconds)
        {
            T item = (T)cache.Get(CacheItem);

            if (item == null)
            {
                cache.Add(CacheItem, new T(), dt_default);
            }
            return (T)cache[CacheItem];
        }

        public bool Contains(string CacheItem)
        {
            return cache.Contains(CacheItem);
        }
    }
}
