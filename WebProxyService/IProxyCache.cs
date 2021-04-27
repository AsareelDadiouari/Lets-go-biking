using System;

namespace WebProxyService
{
    interface ICache<T>
    {
        T Get(string CacheItem);
        T Get(string CacheItem, double dt_seconds);
        T Get(string CacheItem, DateTimeOffset d);
    }
}
