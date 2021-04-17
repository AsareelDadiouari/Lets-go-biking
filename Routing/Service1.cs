using Newtonsoft.Json;
using Routing.JSONClasses;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Device.Location;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Caching;
using WebProxyService;
using WebProxyService.JSONClasses;
using static WebProxyService.Service1;

namespace Routing
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
        private List<Station> allStations;
        private Town.TownJson clientAdress;
        private Station closestDepartureStation, closestArrivalStation;
        string responseDataFromAllStation, start, end;
        static string urlWebProxyStations;
        readonly string apiKey;
        System.ServiceModel.Web.WebOperationContext ctx;

        public Service1()
        {
            urlWebProxyStations = "http://localhost:8733/Design_Time_Addresses/WebProxyService/Service1/rest/Stations";
            apiKey = "5b3ce3597851110001cf6248689d473c044c43afb6cec015efc2fcc1";
            ctx = System.ServiceModel.Web.WebOperationContext.Current;

            using (WebClient webClient = new WebClient())
            {
                this.responseDataFromAllStation = webClient.DownloadString(urlWebProxyStations);
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
            start = departure;
            end = departure;

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

            if (!findClosestStations(clientStart, clientEnd))
                return Task.FromResult(new List<Geo.GeoJson>());

            if (closestArrivalStation != null && closestDepartureStation != null)
            {
                string fromStartClientToDepartStation = "https://api.openrouteservice.org/v2/directions/driving-car?api_key=" + apiKey +
                "&start=" + clientStart.longitude + "," + clientStart.latitude + "&end=" +
                this.closestDepartureStation.position.longitude + "," + this.closestDepartureStation.position.latitude;

                string fromDepartStationToEndStation = "https://api.openrouteservice.org/v2/directions/driving-car?api_key=" + apiKey +
                 "&start=" + this.closestDepartureStation.position.longitude + "," + this.closestDepartureStation.position.latitude
                 + "&end=" + this.closestArrivalStation.position.longitude + "," + this.closestArrivalStation.position.latitude;

                string fromEndStationToEndClient = "https://api.openrouteservice.org/v2/directions/driving-car?api_key=" + apiKey +
                 "&start=" + this.closestArrivalStation.position.longitude + "," + this.closestArrivalStation.position.latitude + "&end=" +
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

        private bool findClosestStations(Coord clientAdressStart, Coord clientAdressEnd)
        {
            CultureInfo info = new CultureInfo("fa-IR");
            info.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = info;
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            this.allStations = JsonConvert.DeserializeObject<List<Station>>(responseDataFromAllStation);

            GeoCoordinate clientstartCoordinate = new GeoCoordinate(
                double.Parse(clientAdressStart.latitude, info),
                double.Parse(clientAdressStart.longitude, info));

            GeoCoordinate clientendCoordinate = new GeoCoordinate(
                double.Parse(clientAdressEnd.latitude, info),
                double.Parse(clientAdressEnd.longitude, info));

            this.allStations.Sort((a, b) => new GeoCoordinate(a.position.latitude, a.position.longitude)
            .GetDistanceTo(new GeoCoordinate(double.Parse(clientAdressStart.latitude, info), double.Parse(clientAdressStart.longitude, info)))
            .CompareTo(new GeoCoordinate(b.position.latitude, b.position.longitude)
            .GetDistanceTo(new GeoCoordinate(double.Parse(clientAdressStart.latitude, info), double.Parse(clientAdressStart.longitude, info)))));

            using (WebClient webClient = new WebClient())
            {
                foreach (Station station in this.allStations)
                {
                    try
                    {
                        string url = "http://localhost:8733/Design_Time_Addresses/WebProxyService/Service1/rest/Station?contract=" +
                        station.contractName + "&id=" + station.number;
                        string responseData = webClient.DownloadString(url);
                        Station stationCheck = JsonConvert.DeserializeObject<Station>(responseData);

                        double distanceToDepartStation = clientstartCoordinate.GetDistanceTo(new GeoCoordinate(stationCheck.position.latitude,
                            stationCheck.position.longitude));

                        double departureToEnd = clientstartCoordinate.GetDistanceTo(clientendCoordinate);
                        File.WriteAllText("C:\\Users\\dyiem\\OneDrive\\Bureau\\e.txt", distanceToDepartStation.ToString() + "\n" + departureToEnd.ToString());

                        if ( departureToEnd < distanceToDepartStation )
                        {
                            //File.WriteAllText("C:\\Users\\dyiem\\OneDrive\\Bureau\\e.txt", departureToEnd.ToString() + "\n" + distanceToDepartStation.ToString());
                            this.closestDepartureStation = null;
                            this.closestArrivalStation = null;
                            return true;
                        }
                        else
                        {
                            this.closestDepartureStation = stationCheck;
                        }
                        break;
                    }
                    catch (Exception e)
                    {
                        this.closestDepartureStation = null;
                    }
                }
            }

            this.allStations.Sort((a, b) => new GeoCoordinate(a.position.latitude, a.position.longitude)
            .GetDistanceTo(new GeoCoordinate(double.Parse(clientAdressEnd.latitude, info), double.Parse(clientAdressEnd.longitude, info)))
            .CompareTo(new GeoCoordinate(b.position.latitude, b.position.longitude)
            .GetDistanceTo(new GeoCoordinate(double.Parse(clientAdressEnd.latitude, info), double.Parse(clientAdressEnd.longitude, info)))));

            using (WebClient webClient = new WebClient())
            {
                foreach (Station station in this.allStations)
                {
                    try
                    {
                        string url = "http://localhost:8733/Design_Time_Addresses/WebProxyService/Service1/rest/Station?ville=" +
                        station.contractName + "&id=" + station.number;
                        string responseData = webClient.DownloadString(url);
                        Station stationCheck = JsonConvert.DeserializeObject<Station>(responseData);
                        if (stationCheck.status == "OPEN" && station.mainStands.availabilities.bikes >= 1)
                        {
                            this.closestArrivalStation = stationCheck;
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        closestArrivalStation = null;
                    }
                }
            }

            return true;
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
