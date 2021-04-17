using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WebProxyService.JSONClasses;

namespace WebProxyService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
        ProxyCache<Station> cache;
        string apiKey = "86be9cea843de8454702fe1727d19b4d8716012a";
        System.ServiceModel.Web.WebOperationContext ctx;

        public Service1()
        {
            cache = new ProxyCache<Station>();
            ctx = System.ServiceModel.Web.WebOperationContext.Current;
        }

        public Task<List<Station>> GetStationsByContractName(string contractName)
        {
            ctx.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            ctx.OutgoingResponse.Headers.Add("Access-Control-Allow-Methods", "GET, PUT, POST, DELETE, HEAD, OPTIONS");
            string url = "https://api.jcdecaux.com/vls/v3/stations?contract="+ contractName + "&apiKey=" + apiKey;

            using (WebClient webClient = new WebClient())
            {
                try
                {
                    string data = webClient.DownloadString(url);
                    List<Station> myDeserializedClass = JsonConvert.DeserializeObject<List<Station>>(data);
                    List<Station> records = new List<Station>();
                    foreach (var record in myDeserializedClass)
                    {
                        if (record.status == "OPEN")
                        {
                            records.Add(record);
                        }
                    }
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
            string url = "https://api.jcdecaux.com/vls/v3/stations/" + stationId.ToString() + "?contract=" + contractName + "&apiKey=" + apiKey;
            ctx.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            ctx.OutgoingResponse.Headers.Add("Access-Control-Allow-Methods", "GET, PUT, POST, DELETE, HEAD, OPTIONS");
            using(WebClient webClient = new WebClient())
            {
                if (!cache.Contains(stationId.ToString()))
                {
                    try
                    {
                        string response = webClient.DownloadString(url);
                        Station station = JsonConvert.DeserializeObject<Station>(response);
                        cache.Add(stationId.ToString(), station);
                    }
                    catch (Exception e)
                    {
                        cache.Add(stationId.ToString(), new Station());
                    }
                }
            }
            return Task.FromResult(cache.Get(stationId.ToString()));
        }

        public Task<List<Station>> GetStations()
        {
            ctx.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            ctx.OutgoingResponse.Headers.Add("Access-Control-Allow-Methods", "GET, PUT, POST, DELETE, HEAD, OPTIONS");
            string url = "https://api.jcdecaux.com/vls/v3/stations?apiKey=" + apiKey;

            using(WebClient webClient = new WebClient())
            {
                try
                {
                    string data = webClient.DownloadString(url);
                    List<Station> myDeserializedClass = JsonConvert.DeserializeObject<List<Station>>(data);
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
