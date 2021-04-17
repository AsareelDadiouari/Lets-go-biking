using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProxyService
{
    interface ICache<T>
    {
         T Get(string CacheItem);
         T Get(string CacheItem, double dt_seconds);
         T Get(string CacheItem, DateTimeOffset d);

        void Add(string CacheItem, T data);
    }
}
