using System;
using System.Net;
using System.Runtime.Caching;
using Newtonsoft.Json;
using WebProxyService.JSONClasses;

namespace WebProxyService
{
    public class ProxyCache<T> : ICache<T> where T : new()
    {
        private readonly string apiKey = "86be9cea843de8454702fe1727d19b4d8716012a";
        private readonly ObjectCache cache;

        public ProxyCache()
        {
            cache = MemoryCache.Default;
        }

        public T Get(string CacheItem)
        {
            string stationId = CacheItem.Split('-')[0], contractName = CacheItem.Split('-')[1];
            var url = "https://api.jcdecaux.com/vls/v3/stations/" + stationId + "?contract=" + contractName +
                      "&apiKey=" + apiKey;

            var item = (T) cache.Get(stationId);
            if (item == null)
                using (var webClient = new WebClient())
                {
                    Station station = null;
                    if (!cache.Contains(stationId))
                        try
                        {
                            var response = webClient.DownloadString(url);
                            station = JsonConvert.DeserializeObject<Station>(response);
                        }
                        catch (Exception)
                        {
                            station = new Station();
                        }

                    var cacheItemPolicy = new CacheItemPolicy
                    {
                        AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration,
                        Priority = CacheItemPriority.Default
                    };
                    cache.Set(stationId, station, cacheItemPolicy);
                }

            return (T) cache[stationId];
        }

        public T Get(string CacheItem, DateTimeOffset dt)
        {
            string stationId = CacheItem.Split('-')[0], contractName = CacheItem.Split('-')[1];
            var url = "https://api.jcdecaux.com/vls/v3/stations/" + stationId + "?contract=" + contractName +
                      "&apiKey=" + apiKey;

            var item = (T) cache.Get(stationId);
            if (item == null)
                using (var webClient = new WebClient())
                {
                    Station station = null;
                    if (!cache.Contains(stationId))
                        try
                        {
                            var response = webClient.DownloadString(url);
                            station = JsonConvert.DeserializeObject<Station>(response);
                        }
                        catch (Exception )
                        {
                            station = new Station();
                        }

                    var cacheItemPolicy = new CacheItemPolicy
                    {
                        AbsoluteExpiration = dt,
                        Priority = CacheItemPriority.Default
                    };
                    cache.Set(stationId, station, cacheItemPolicy);
                }

            return (T) cache[stationId];
            ;
        }

        public T Get(string CacheItem, double dt_seconds)
        {
            string stationId = CacheItem.Split('-')[0], contractName = CacheItem.Split('-')[1];
            var url = "https://api.jcdecaux.com/vls/v3/stations/" + stationId + "?contract=" + contractName +
                      "&apiKey=" + apiKey;

            var item = (T) cache.Get(stationId);
            if (item == null)
                using (var webClient = new WebClient())
                {
                    Station station = null;
                    if (!cache.Contains(stationId))
                        try
                        {
                            var response = webClient.DownloadString(url);
                            station = JsonConvert.DeserializeObject<Station>(response);
                        }
                        catch (Exception)
                        {
                            station = new Station();
                        }

                    var cacheItemPolicy = new CacheItemPolicy
                    {
                        AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(dt_seconds),
                        Priority = CacheItemPriority.Default
                    };
                    cache.Set(stationId, station, cacheItemPolicy);
                }

            return (T) cache[stationId];
        }
    }
}