using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Device.Location;
using System.Globalization;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Routing.JSONClasses;
using WebProxyService.JSONClasses;

namespace Routing
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
        private static string urlWebProxyStations;
        private readonly List<Station> allStations;
        private readonly string apiKey;
        private readonly WebOperationContext ctx;
        private readonly string responseDataFromAllStation;
        private readonly List<GeoCoordinate> StationsGeocoordinates;
        private Town.TownJson clientAdress;
        private Station closestDepartureStation, closestArrivalStation;

        public Service1()
        {
            urlWebProxyStations = "http://localhost:8733/Design_Time_Addresses/WebProxyService/Service1/rest/Stations";
            apiKey = "5b3ce3597851110001cf6248689d473c044c43afb6cec015efc2fcc1";
            ctx = WebOperationContext.Current;

            using (var webClient = new WebClient())
            {
                responseDataFromAllStation = webClient.DownloadString(urlWebProxyStations);
                allStations = JsonConvert.DeserializeObject<List<Station>>(responseDataFromAllStation);
                StationsGeocoordinates = convertStationsToGeo(allStations);
            }

            closestDepartureStation = new Station();
            closestArrivalStation = new Station();
        }

        public Task<List<Geo.GeoJson>> GetGeoData(string departure, string arrival)
        {
            departure = departure.Trim();
            arrival = arrival.Trim();

            if (departure.Length == 0 || arrival.Length == 0) return Task.FromResult(new List<Geo.GeoJson>());

            var dep = getCoord(departure);
            var arr = getCoord(arrival);

            if (dep == null || arr == null) return Task.FromResult(new List<Geo.GeoJson>());

            var clientStart = new Coord(dep);
            var clientEnd = new Coord(arr);

            findClosestStations(
                new GeoCoordinate(double.Parse(clientStart.latitude, CultureInfo.InvariantCulture),
                    double.Parse(clientStart.longitude, CultureInfo.InvariantCulture)),
                new GeoCoordinate(double.Parse(clientEnd.latitude, CultureInfo.InvariantCulture),
                    double.Parse(clientEnd.longitude, CultureInfo.InvariantCulture)));

            if (closestArrivalStation != null && closestDepartureStation != null)
            {
                var fromStartClientToDepartStation =
                    "https://api.openrouteservice.org/v2/directions/foot-walking?api_key=" + apiKey +
                    "&start=" + clientStart.longitude + "," + clientStart.latitude + "&end=" +
                    closestDepartureStation.position.longitude.ToString().Replace(",", ".") + "," +
                    closestDepartureStation.position.latitude.ToString().Replace(",", ".");

                var fromDepartStationToEndStation =
                    "https://api.openrouteservice.org/v2/directions/cycling-road?api_key=" + apiKey +
                    "&start=" + closestDepartureStation.position.longitude.ToString().Replace(",", ".") + "," +
                    closestDepartureStation.position.latitude.ToString().Replace(",", ".")
                    + "&end=" + closestArrivalStation.position.longitude.ToString().Replace(",", ".") + "," +
                    closestArrivalStation.position.latitude.ToString().Replace(",", ".");

                var fromEndStationToEndClient = "https://api.openrouteservice.org/v2/directions/foot-walking?api_key=" +
                                                apiKey +
                                                "&start=" +
                                                closestArrivalStation.position.longitude.ToString().Replace(",", ".") +
                                                "," + closestArrivalStation.position.latitude.ToString()
                                                    .Replace(",", ".") + "&end=" +
                                                clientEnd.longitude + "," + clientEnd.latitude;

                using (var webClient = new WebClient())
                {
                    Geo.GeoJson data2 = new Geo.GeoJson(), data3 = new Geo.GeoJson();
                    try
                    {
                        var responseData2 = webClient.DownloadString(fromDepartStationToEndStation);
                        data2 = JsonConvert.DeserializeObject<Geo.GeoJson>(responseData2);
                    }
                    catch (Exception e)
                    {
                        data2 = new Geo.GeoJson();
                    }

                    try
                    {
                        var responseData3 = webClient.DownloadString(fromEndStationToEndClient);
                        data3 = JsonConvert.DeserializeObject<Geo.GeoJson>(responseData3);
                    }
                    catch (Exception e)
                    {
                        data3 = new Geo.GeoJson();
                    }

                    try
                    {
                        ctx.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
                        ctx.OutgoingResponse.Headers.Add("Access-Control-Allow-Methods",
                            "GET, PUT, POST, DELETE, HEAD, OPTIONS");
                        var responseData1 = webClient.DownloadString(fromStartClientToDepartStation);
                        var data = JsonConvert.DeserializeObject<Geo.GeoJson>(responseData1);
                        var glob = new List<Geo.GeoJson>();
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
            }

            var fromStartClientToEndClient = "https://api.openrouteservice.org/v2/directions/driving-car?api_key=" +
                                             apiKey +
                                             "&start=" + clientStart.longitude + "," + clientStart.latitude + "&end=" +
                                             clientEnd.longitude + "," + clientEnd.latitude;

            using (var webClient = new WebClient())
            {
                ctx.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
                ctx.OutgoingResponse.Headers.Add("Access-Control-Allow-Methods",
                    "GET, PUT, POST, DELETE, HEAD, OPTIONS");
                var geoJsons = new List<Geo.GeoJson>();
                try
                {
                    var responseData = webClient.DownloadString(fromStartClientToEndClient);
                    var data = JsonConvert.DeserializeObject<Geo.GeoJson>(responseData);
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

        private static void OnSendingRequest(object sender, SendingRequestEventArgs e)
        {
            e.RequestHeaders.Add("Access-Control-Allow-Origin", "*");
        }

        private Task<string> getCoord(string adress)
        {
            if (IsDigitsOnly(adress)) return Task.FromResult(adress);
            var geoCallurl = "https://api-adresse.data.gouv.fr/search/?q=" + adress + "&limit=1";

            using (var webClient = new WebClient())
            {
                try
                {
                    var data = webClient.DownloadString(geoCallurl);
                    clientAdress = JsonConvert.DeserializeObject<Town.TownJson>(data);

                    var longitude = clientAdress.features[0].geometry.coordinates[0];
                    var latitude = clientAdress.features[0].geometry.coordinates[1];

                    return Task.FromResult(longitude.ToString().Replace(',', '.') + "," +
                                           latitude.ToString().Replace(',', '.'));
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        private void findClosestStations(GeoCoordinate start, GeoCoordinate end)
        {
            var sortedFromStart = StationsGeocoordinates.OrderBy(x => x.GetDistanceTo(start)).ToList();
            var sortedFromEnd = StationsGeocoordinates.OrderBy(x => x.GetDistanceTo(end)).ToList();

            var indice = 0;
            while (indice < allStations.Count)
            {
                var pos = new Position(sortedFromStart[indice].Latitude, sortedFromStart[indice].Longitude);
                var tempDep = allStations.Find(x => x.position.Equals(pos));
                using (var webClient = new WebClient())
                {
                    try
                    {
                        var url =
                            "http://localhost:8733/Design_Time_Addresses/WebProxyService/Service1/rest/Station?contract=" +
                            tempDep.contractName + "&id=" + tempDep.number;
                        var response = webClient.DownloadString(url);
                        var sta = JsonConvert.DeserializeObject<Station>(response);
                        if (sta.status == "OPEN" && sta.mainStands.availabilities.bikes >= 1)
                        {
                            closestDepartureStation = sta;

                            if (start.GetDistanceTo(new GeoCoordinate(sta.position.latitude, sta.position.longitude)) >
                                start.GetDistanceTo(end))
                            {
                                closestDepartureStation = null;
                                closestArrivalStation = null;
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

            if (closestDepartureStation != null)
            {
                indice = 0;
                while (indice < allStations.Count)
                {
                    var pos = new Position(sortedFromEnd[indice].Latitude, sortedFromEnd[indice].Longitude);
                    var tempDep = allStations.Find(x => x.position.Equals(pos));
                    using (var webClient = new WebClient())
                    {
                        try
                        {
                            var url =
                                "http://localhost:8733/Design_Time_Addresses/WebProxyService/Service1/rest/Station?contract=" +
                                tempDep.contractName + "&id=" + tempDep.number;
                            var response = webClient.DownloadString(url);
                            var sta = JsonConvert.DeserializeObject<Station>(response);
                            if (sta.status == "OPEN" && sta.mainStands.availabilities.stands >= 1)
                            {
                                closestArrivalStation = sta;
                                if (closestDepartureStation.address.Equals(closestArrivalStation.address))
                                {
                                    closestArrivalStation = null;
                                    closestDepartureStation = null;
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
        }

        private List<GeoCoordinate> convertStationsToGeo(List<Station> stations)
        {
            var geos = new List<GeoCoordinate>();

            foreach (var station in stations)
                geos.Add(new GeoCoordinate(station.position.latitude, station.position.longitude));

            return geos;
        }

        private static bool IsDigitsOnly(string str)
        {
            foreach (var c in str)
                if (c < '0' || c > '9' || c != '.' && c != ',')
                    return false;
            return true;
        }
    }
}