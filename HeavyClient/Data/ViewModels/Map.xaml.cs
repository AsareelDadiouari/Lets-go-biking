using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using Google.Cloud.Firestore;
using HeavyClient.Config;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Maps.MapControl.WPF;
using Routing;

namespace HeavyClient.Data.ViewModels
{
    /// <summary>
    ///     Interaction logic for Map.xaml
    /// </summary>
    public partial class Map : Page
    {
        public static List<StationStatistics> statsToSave = new List<StationStatistics>();
        private readonly string configURL;
        private readonly FirestoreDb database;
        private readonly GeoGeoJson[] geoJsons;
        private Service1Client service1;

        public Map(GeoGeoJson[] geoJsons)
        {
            InitializeComponent();
            service1 = new Service1Client();
            configURL = AppDomain.CurrentDomain.BaseDirectory + "\\config.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", configURL);
            database = FirestoreDb.Create("let-s-go-biking");
            this.geoJsons = geoJsons;

            //Setting default view
            MyMap.SetView(new Location(this.geoJsons[0].features[0].geometry.coordinates[0][1],
                this.geoJsons[0].features[0].geometry.coordinates[0][0]), 15);
            //MyMap.Mode = new AerialMode(false);
            MapSetup();
            DetailsSetup();
        }

        private void MapSetup()
        {
            var locs = new LocationCollection();
            var mapPolylines = new List<MapPolyline>();
            var pins = new List<Pushpin>();

            foreach (var data in geoJsons.Select((value, index) => new {value, index}))
            foreach (var feature in data.value.features.Select((value, index) => new {value, index}))
            {
                switch (data.index)
                {
                    case 0:

                        var routeLine = new MapPolyline
                        {
                            Stroke = new SolidColorBrush(Colors.Red),
                            StrokeThickness = 4
                        };
                        routeLine.Locations = new LocationCollection();

                        foreach (var loc in feature.value.geometry.coordinates)
                            routeLine.Locations.Add(new Location(loc[1], loc[0]));

                        mapPolylines.Add(routeLine);

                        var pin = new Pushpin
                        {
                            Location = new Location(feature.value.geometry.coordinates[0][1],
                                feature.value.geometry.coordinates[0][0]),
                            ToolTip = "Departure" 
                        };

                            var geo = this.geoJsons.Where(x => x.features[0].geometry.coordinates[0][1]
                        .Equals(feature.value.geometry.coordinates[0][1]) && x.features[0].geometry.coordinates[0][0]
                        .Equals(feature.value.geometry.coordinates[0][0])).Single();
                            if (geo.station != null)
                                DepartureStation.Content = geo.station.name;
                            pins.Add(pin);
                        break;
                    case 1:

                        var routeLine2 = new MapPolyline
                        {
                            Stroke = new SolidColorBrush(Colors.Yellow),
                            StrokeThickness = 4
                        };
                        routeLine2.Locations = new LocationCollection();

                        foreach (var loc in feature.value.geometry.coordinates)
                            routeLine2.Locations.Add(new Location(loc[1], loc[0]));

                        mapPolylines.Add(routeLine2);

                        var pin1 = new Pushpin
                        {
                            Location = new Location(feature.value.geometry.coordinates[0][1],
                                feature.value.geometry.coordinates[0][0]),
                            ToolTip = "Departure Station"
                        };
       
                        pins.Add(pin1);
                        break;
                    case 2:

                        var routeLine3 = new MapPolyline
                        {
                            Stroke = new SolidColorBrush(Colors.Green),
                            StrokeThickness = 4
                        };
                        routeLine3.Locations = new LocationCollection();

                        foreach (var loc in feature.value.geometry.coordinates)
                            routeLine3.Locations.Add(new Location(loc[1], loc[0]));
                        
                        mapPolylines.Add(routeLine3);

                        var pin2 = new Pushpin
                        {
                            Location = new Location(feature.value.geometry.coordinates[0][1],
                                feature.value.geometry.coordinates[0][0]),
                            ToolTip = "Arrival Station"
                        };
                            var geo1 = this.geoJsons.Where(x => x.features[0].geometry.coordinates[0][1]
                                .Equals(feature.value.geometry.coordinates[0][1]) && x.features[0].geometry.coordinates[0][0]
                                .Equals(feature.value.geometry.coordinates[0][0])).Single();

                            if (geo1.station != null)
                                ArrivalStation.Content = geo1.station.name;

                        pins.Add(pin2);
                        break;
                }

                foreach (var coordinate in feature.value.geometry.coordinates)
                    locs.Add(new Location(coordinate[1], coordinate[0]));
            }

            //Adding pins and routes with different colours
            for (var i = 0; i < mapPolylines.Count; i++) MyMap.Children.Add(mapPolylines[i]);

            for (var i = 0; i < pins.Count; i++) MyMap.Children.Add(pins[i]);
            //Adding last pin
            var pinFinal = new Pushpin
            {
                Location = locs[locs.Count - 1],
                ToolTip = "Arrival"
            };

            if (pins.Count == 1)
            {
                depStackPane.Visibility = System.Windows.Visibility.Collapsed;
                arrStackPane.Visibility = System.Windows.Visibility.Collapsed;
            }

            MyMap.Children.Add(pinFinal);
        }

        private async void DetailsSetup()
        {
            double dur = 0, dist = 0;
            //Set Steps
            foreach (var element in geoJsons.Select((value, index) => new {value, index}))
            foreach (var segment in element.value.features[0].properties.segments)
            {
                foreach (var step in segment.steps)
                    {
                        byte[] bytes = Encoding.Default.GetBytes(step.instruction);

                        directions.Items.Add(new ListBoxItem
                        {
                            Content = Encoding.UTF8.GetString(bytes),
                            FontSize = 13,
                        });
                    }
                   

                dur += segment.duration;
                dist += segment.distance;
            }


            Distance.Content = dist / 1000 + "km";
            Duration.Content = dur / 3600 + "h";

            MainWindow.routeSearches.Add(Distance.Content + "*" + DateTime.Now + "*" + Duration.Content);

            var lastSize = geoJsons[geoJsons.Length - 1].features[0].properties.segments[0].steps.Length - 1;
            DepartAdress.Content = geoJsons[0].features[0].properties.segments[0].steps[0].name;
            ArriveAdress.Content = geoJsons[geoJsons.Length - 1].features[0]
                .properties.segments[0].steps[lastSize - 1].name;

            mostVDeparture.Content = await GetMostUsedDepStation();
            mostVArrival.Content = await GetMostUsedArrStation();

            var stations = await GetFiveMostUsed();

            var seriesCollection = new SeriesCollection();

            foreach (var station in stations)
                seriesCollection.Add(new ColumnSeries
                {
                    Title = station.Key,
                    Values = new ChartValues<int> {station.Value},
                    Fill = Brushes.Blue
                });
            chart.Series.AddRange(seriesCollection);
        }

        private async Task<string> GetMostUsedDepStation()
        {
            var stationsDeparture = database.Collection("StationsDeparture");
            var snapshot = await stationsDeparture.GetSnapshotAsync();

            var found = snapshot.Documents.ToList()
                .OrderByDescending(x => x.ConvertTo<StationStatistics>().occurence).First();

            var foundStation = found.ConvertTo<StationStatistics>();
            return foundStation.station.name + "," + foundStation.station.contractName;
        }

        private async Task<string> GetMostUsedArrStation()
        {
            var stationsArrival = database.Collection("StationsArrival");
            var snapshot = await stationsArrival.GetSnapshotAsync();

            var found = snapshot.Documents.ToList()
                .OrderByDescending(x => x.ConvertTo<StationStatistics>().occurence).First();

            var foundStation = found.ConvertTo<StationStatistics>();
            return foundStation.station.name + "," + foundStation.station.contractName;
        }

        private async Task<Dictionary<string, int>> GetFiveMostUsed()
        {
            var pairs = new Dictionary<string, int>();

            var stationsDeparture = database.Collection("StationsDeparture");
            var snapshotDeparture = await stationsDeparture.GetSnapshotAsync();

            var stationsArrival = database.Collection("StationsArrival");
            var snapshotArrival = await stationsArrival.GetSnapshotAsync();

            var documents = snapshotDeparture.Documents.ToList().Union(snapshotArrival.Documents.ToList())
                .OrderByDescending(x => x.ConvertTo<StationStatistics>().occurence).GroupBy(x => x.Id).Select(y => y.First());

            foreach (var doc in documents.Take(5))
            {
                var currentStation = doc.ConvertTo<StationStatistics>();
                if (statsToSave.Count < 5)
                    statsToSave.Add(currentStation);
                pairs.Add(currentStation.station.name + "\n[" + currentStation.station.contractName + "]",
                    currentStation.occurence);
            }

            foreach (var doc in documents.Skip(5))
            {
                var currentStation = doc.ConvertTo<StationStatistics>();
                statsToSave.Add(currentStation);
            }

            return pairs;
        }
    }
}