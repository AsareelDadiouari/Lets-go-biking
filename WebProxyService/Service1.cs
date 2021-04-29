using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using WebProxyService.JSONClasses;

namespace WebProxyService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
        private readonly string apiKey = "86be9cea843de8454702fe1727d19b4d8716012a";
        private readonly ProxyCache<Station> cache;
        private readonly WebOperationContext ctx;

        public Service1()
        {
            cache = new ProxyCache<Station>();
            ctx = WebOperationContext.Current;
        }

        public Task<List<Station>> GetStationsByContractName(string contractName)
        {
            ctx.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            ctx.OutgoingResponse.Headers.Add("Access-Control-Allow-Methods", "GET, PUT, POST, DELETE, HEAD, OPTIONS");
            var url = "https://api.jcdecaux.com/vls/v3/stations?contract=" + contractName + "&apiKey=" + apiKey;

            using (var webClient = new WebClient())
            {
                try
                {
                    var data = webClient.DownloadString(url);
                    var myDeserializedClass = JsonConvert.DeserializeObject<List<Station>>(data);
                    var records = new List<Station>();
                    foreach (var record in myDeserializedClass)
                        if (record.status == "OPEN")
                            records.Add(record);
                    myDeserializedClass = records;
                    return Task.FromResult(myDeserializedClass);
                }
                catch (Exception e)
                {
                    return Task.FromResult(new List<Station>());
                }
            }
        }

        public Task<Station> GetOneStation(string contractName, int stationId)
        {
            ctx.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            ctx.OutgoingResponse.Headers.Add("Access-Control-Allow-Methods", "GET, PUT, POST, DELETE, HEAD, OPTIONS");
            return Task.FromResult(cache.Get(stationId + "-" + contractName));
        }

        public Task<List<Station>> GetStations()
        {
            ctx.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            ctx.OutgoingResponse.Headers.Add("Access-Control-Allow-Methods", "GET, PUT, POST, DELETE, HEAD, OPTIONS");
            var url = "https://api.jcdecaux.com/vls/v3/stations?apiKey=" + apiKey;

            using (var webClient = new WebClient())
            {
                try
                {
                    var data = webClient.DownloadString(url);
                    var myDeserializedClass = JsonConvert.DeserializeObject<List<Station>>(data);
                    return Task.FromResult(myDeserializedClass);
                }
                catch (Exception e)
                {
                    return Task.FromResult(new List<Station>());
                }
            }
        }
    }
}