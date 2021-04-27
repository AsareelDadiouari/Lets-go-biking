using Newtonsoft.Json;
using System;
using System.Net;
using System.Runtime.Caching;
using WebProxyService.JSONClasses;

namespace WebProxyService
{
    public class ProxyCache<T> : ICache<T> where T : new()
    {
        ObjectCache cache;
        string apiKey = "86be9cea843de8454702fe1727d19b4d8716012a";

        public ProxyCache()
        {
            cache = MemoryCache.Default;
        }

        public T Get(string CacheItem)
        {
            string stationId = CacheItem.Split('-')[0], contractName = CacheItem.Split('-')[1];
            string url = "https://api.jcdecaux.com/vls/v3/stations/" + stationId.ToString() + "?contract=" + contractName + "&apiKey=" + apiKey;

            T item = (T)cache.Get(stationId);
            if (item == null)
            {
                using (WebClient webClient = new WebClient())
                {
                    Station station = null;
                    if (!cache.Contains(stationId.ToString()))
                    {
                        try
                        {
                            string response = webClient.DownloadString(url);
                            station = JsonConvert.DeserializeObject<Station>(response);
                        }
                        catch (Exception e)
                        {
                            station = new Station();
                        }
                    }
                    CacheItemPolicy cacheItemPolicy = new CacheItemPolicy
                    {
                        AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration,
                        Priority = CacheItemPriority.Default
                    };
                    cache.Set(stationId, station, cacheItemPolicy);
                }
            }
            return (T)cache[stationId];
        }

        public T Get(string CacheItem, DateTimeOffset dt)
        {
            string stationId = CacheItem.Split('-')[0], contractName = CacheItem.Split('-')[1];
            string url = "https://api.jcdecaux.com/vls/v3/stations/" + stationId.ToString() + "?contract=" + contractName + "&apiKey=" + apiKey;

            T item = (T)cache.Get(stationId);
            if (item == null)
            {
                using (WebClient webClient = new WebClient())
                {
                    Station station = null;
                    if (!cache.Contains(stationId.ToString()))
                    {
                        try
                        {
                            string response = webClient.DownloadString(url);
                            station = JsonConvert.DeserializeObject<Station>(response);
                        }
                        catch (Exception e)
                        {
                            station = new Station();
                        }
                    }
                    CacheItemPolicy cacheItemPolicy = new CacheItemPolicy
                    {
                        AbsoluteExpiration = dt,
                        Priority = CacheItemPriority.Default
                    };
                    cache.Set(stationId, station, cacheItemPolicy);
                }
            }
            return (T)cache[stationId]; ;
        }

        public T Get(string CacheItem, double dt_seconds)
        {
            string stationId = CacheItem.Split('-')[0], contractName = CacheItem.Split('-')[1];
            string url = "https://api.jcdecaux.com/vls/v3/stations/" + stationId.ToString() + "?contract=" + contractName + "&apiKey=" + apiKey;

            T item = (T)cache.Get(stationId);
            if (item == null)
            {
                using (WebClient webClient = new WebClient())
                {
                    Station station = null;
                    if (!cache.Contains(stationId.ToString()))
                    {
                        try
                        {
                            string response = webClient.DownloadString(url);
                            station = JsonConvert.DeserializeObject<Station>(response);
                        }
                        catch (Exception e)
                        {
                            station = new Station();
                        }
                    }
                    CacheItemPolicy cacheItemPolicy = new CacheItemPolicy
                    {
                        AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(dt_seconds),
                        Priority = CacheItemPriority.Default
                    };
                    cache.Set(stationId, station, cacheItemPolicy);
                }
            }
            return (T)cache[stationId];
        }
    }
}
