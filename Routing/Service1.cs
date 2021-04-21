using Newtonsoft.Json;
using Routing.JSONClasses;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Device.Location;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebProxyService.JSONClasses;

namespace Routing
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
        private List<Station> allStations;
        private Town.TownJson clientAdress;
        private Station closestDepartureStation, closestArrivalStation;
        string responseDataFromAllStation;
        static string urlWebProxyStations;
        readonly string apiKey;
        List<GeoCoordinate> StationsGeocoordinates;
        System.ServiceModel.Web.WebOperationContext ctx;

        public Service1()
        {
            urlWebProxyStations = "http://localhost:8733/Design_Time_Addresses/WebProxyService/Service1/rest/Stations";
            apiKey = "5b3ce3597851110001cf6248689d473c044c43afb6cec015efc2fcc1";
            ctx = System.ServiceModel.Web.WebOperationContext.Current;

            using (WebClient webClient = new WebClient())
            {
                this.responseDataFromAllStation = webClient.DownloadString(urlWebProxyStations);
                this.allStations = JsonConvert.DeserializeObject<List<Station>>(responseDataFromAllStation);
                this.StationsGeocoordinates = convertStationsToGeo(this.allStations);
            }
        }

        private static void OnSendingRequest(object sender, SendingRequestEventArgs e)
        {
            e.RequestHeaders.Add("Access-Control-Allow-Origin", "*");
        }

        public string getContract()
        {
            ctx.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            ctx.OutgoingResponse.Headers.Add("Access-Control-Allow-Methods", "GET, PUT, POST, DELETE, HEAD, OPTIONS");
            return "bro";
        }

        public Task<List<Geo.GeoJson>> GetGeoData(string departure, string arrival)
        {
            departure = departure.Trim();
            arrival = arrival.Trim();

            if (departure.Length == 0 || arrival.Length == 0)
            {
                return Task.FromResult(new List<Geo.GeoJson>());
            }

            Task<string> dep = getCoord(departure);
            Task<string> arr = getCoord(arrival);

            if (dep == null || arr == null)
            {
                return Task.FromResult(new List<Geo.GeoJson>());
            }

            Coord clientStart = new Coord(dep);
            Coord clientEnd = new Coord(arr);

            /*if (!findClosestStations(clientStart, clientEnd))
                return Task.FromResult(new List<Geo.GeoJson>());*/
            
            findClosestStations2(new GeoCoordinate(double.Parse(clientStart.latitude, CultureInfo.InvariantCulture), double.Parse(clientStart.longitude, CultureInfo.InvariantCulture)),
                new GeoCoordinate(double.Parse(clientEnd.latitude, CultureInfo.InvariantCulture), double.Parse(clientEnd.longitude, CultureInfo.InvariantCulture)));

            if (closestArrivalStation != null && closestDepartureStation != null)
            {
                string fromStartClientToDepartStation = "https://api.openrouteservice.org/v2/directions/foot-walking?api_key=" + apiKey +
                "&start=" + clientStart.longitude + "," + clientStart.latitude + "&end=" +
                this.closestDepartureStation.position.longitude.ToString().Replace(",",".") + "," + this.closestDepartureStation.position.latitude.ToString().Replace(",", ".");

                string fromDepartStationToEndStation = "https://api.openrouteservice.org/v2/directions/cycling-road?api_key=" + apiKey +
                 "&start=" + this.closestDepartureStation.position.longitude.ToString().Replace(",", ".") + "," + this.closestDepartureStation.position.latitude.ToString().Replace(",", ".")
                 + "&end=" + this.closestArrivalStation.position.longitude.ToString().Replace(",", ".") + "," + this.closestArrivalStation.position.latitude.ToString().Replace(",", ".");

                string fromEndStationToEndClient = "https://api.openrouteservice.org/v2/directions/foot-walking?api_key=" + apiKey +
                 "&start=" + this.closestArrivalStation.position.longitude.ToString().Replace(",", ".") + "," + this.closestArrivalStation.position.latitude.ToString().Replace(",", ".") + "&end=" +
                 clientEnd.longitude + "," + clientEnd.latitude;

                using (WebClient webClient = new WebClient())
                {
                    Geo.GeoJson data2 = new Geo.GeoJson(),
                        data3 = new Geo.GeoJson();

                    try
                    {
                        string responseData2 = webClient.DownloadString(fromDepartStationToEndStation);
                        data2 = JsonConvert.DeserializeObject<Geo.GeoJson>(responseData2);
                    }
                    catch (Exception e)
                    {
                        data2 = new Geo.GeoJson();
                    }
                    try
                    {
                        string responseData3 = webClient.DownloadString(fromEndStationToEndClient);
                        data3 = JsonConvert.DeserializeObject<Geo.GeoJson>(responseData3);
                    }
                    catch (Exception e)
                    {
                        data3 = new Geo.GeoJson();
                    }
                    try
                    {
                        ctx.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
                        ctx.OutgoingResponse.Headers.Add("Access-Control-Allow-Methods", "GET, PUT, POST, DELETE, HEAD, OPTIONS");
                        string responseData1 = webClient.DownloadString(fromStartClientToDepartStation);
                        Geo.GeoJson data = JsonConvert.DeserializeObject<Geo.GeoJson>(responseData1);
                        List<Geo.GeoJson> glob = new List<Geo.GeoJson>();
                        if (data2.features != null)
                        {
                            data.station = closestDepartureStation;
                            data.waypoints = data.features[0].geometry.coordinates;
                            data2.waypoints = data2.features[0].geometry.coordinates;
                            glob.Add(data);
                            glob.Add(data2);
                        }
                        if (data3.features != null)
                        {
                            data3.station = closestArrivalStation;
                            data3.waypoints = data3.features[0].geometry.coordinates;
                            glob.Add(data3);
                        }

                        return Task.FromResult(glob);
                    }
                    catch (Exception e)
                    {
                        return Task.FromResult(new List<Geo.GeoJson>());
                    }
                }
            } else
            {
                string fromStartClientToEndClient = "https://api.openrouteservice.org/v2/directions/driving-car?api_key=" + apiKey +
                "&start=" + clientStart.longitude + "," + clientStart.latitude + "&end=" +
                clientEnd.longitude + "," + clientEnd.latitude;

                using(WebClient webClient = new WebClient())
                {
                    ctx.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
                    ctx.OutgoingResponse.Headers.Add("Access-Control-Allow-Methods", "GET, PUT, POST, DELETE, HEAD, OPTIONS");
                    List<Geo.GeoJson> geoJsons = new List<Geo.GeoJson>();
                    try
                    {
                        string responseData = webClient.DownloadString(fromStartClientToEndClient);
                        Geo.GeoJson data = JsonConvert.DeserializeObject<Geo.GeoJson>(responseData);
                        data.waypoints = data.features[0].geometry.coordinates;

                        geoJsons.Add(data);
                        return Task.FromResult(geoJsons);
                    }
                    catch (Exception e)
                    {
                        return Task.FromResult(geoJsons);
                    }
                }
            }
        }

        private Task<string> getCoord(string adress)
        {
            if (IsDigitsOnly(adress))
            {
                return Task.FromResult(adress);
            }
            string geoCallurl = "https://api-adresse.data.gouv.fr/search/?q=" + adress + "&limit=1";

            using (WebClient webClient = new WebClient())
            {
                try
                {
                    string data = webClient.DownloadString(geoCallurl);
                    this.clientAdress = JsonConvert.DeserializeObject<Town.TownJson>(data);

                    double longitude = this.clientAdress.features[0].geometry.coordinates[0];
                    double latitude = this.clientAdress.features[0].geometry.coordinates[1];

                    return Task.FromResult(longitude.ToString().Replace(',', '.') + "," + latitude.ToString().Replace(',', '.'));
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        private void findClosestStations2(GeoCoordinate start, GeoCoordinate end)
        {
            var sortedFromStart = StationsGeocoordinates.OrderBy(x => x.GetDistanceTo(start)).ToList();
            var sortedFromEnd = StationsGeocoordinates.OrderBy(x => x.GetDistanceTo(end)).ToList();

            int indice = 0;
            while (indice < this.allStations.Count)
            {
                Position pos = new Position(sortedFromStart[indice].Latitude, sortedFromStart[indice].Longitude);
                Station tempDep = this.allStations.Find(x => x.position.Equals(pos));
                using (WebClient webClient = new WebClient())
                {
                    try
                    {
                        string url = "http://localhost:8733/Design_Time_Addresses/WebProxyService/Service1/rest/Station?contract=" +
                        tempDep.contractName + "&id=" + tempDep.number;
                        string response = webClient.DownloadString(url);
                        Station sta = JsonConvert.DeserializeObject<Station>(response);
                        if (sta.status == "OPEN" && sta.mainStands.availabilities.bikes >=1)
                        {
                            this.closestDepartureStation = sta;
                            break;
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }
                indice++;
            }

            indice = 0;
            while (indice < this.allStations.Count)
            {
                Position pos = new Position(sortedFromEnd[indice].Latitude, sortedFromEnd[indice].Longitude);
                Station tempDep = this.allStations.Find(x => x.position.Equals(pos));
                using (WebClient webClient = new WebClient())
                {
                    try
                    {
                        string url = "http://localhost:8733/Design_Time_Addresses/WebProxyService/Service1/rest/Station?contract=" +
                        tempDep.contractName + "&id=" + tempDep.number;
                        string response = webClient.DownloadString(url);
                        Station sta = JsonConvert.DeserializeObject<Station>(response);
                        if (sta.status == "OPEN" && sta.mainStands.availabilities.stands >= 1)
                        {
                            this.closestArrivalStation = sta;
                            if (this.closestDepartureStation.address.Equals(this.closestArrivalStation.address))
                            {
                                this.closestArrivalStation = null;
                                this.closestDepartureStation = null;
                            }
                            break;
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }
                indice++;
            }
        }

        private List<GeoCoordinate> convertStationsToGeo(List<Station> stations)
        {
            List<GeoCoordinate> geos = new List<GeoCoordinate>();

            foreach(var station in stations)
            {
                geos.Add(new GeoCoordinate(station.position.latitude, station.position.longitude));
            }

            return geos;
        }

        private static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if ((c < '0' || c > '9') || (c != '.' && c != ','))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
