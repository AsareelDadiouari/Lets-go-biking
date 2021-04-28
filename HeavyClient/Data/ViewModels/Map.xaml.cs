using Google.Cloud.Firestore;
using HeavyClient.Config;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Maps.MapControl.WPF;
using Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace HeavyClient.Data.ViewModels
{
    /// <summary>
    /// Interaction logic for Map.xaml
    /// </summary>
    public partial class Map : Page
    {
        GeoGeoJson[] geoJsons;
        Service1Client service1 = new Service1Client();
        FirestoreDb database;
        readonly private string configURL = AppDomain.CurrentDomain.BaseDirectory + "\\config.json";
        public Map(GeoGeoJson[] geoJsons)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", configURL);
            database = FirestoreDb.Create("let-s-go-biking");
            InitializeComponent();
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
            LocationCollection locs = new LocationCollection();
            List<MapPolyline> mapPolylines = new List<MapPolyline>();
            List<Pushpin> pins = new List<Pushpin>();

            foreach (var data in this.geoJsons.Select((value, index) => new { value, index }))
            {
                foreach (var feature in data.value.features.Select((value, index) => new { value, index }))
                {
                    switch (data.index)
                    {
                        case 0:

                            MapPolyline routeLine = new MapPolyline()
                            {
                                Stroke = new SolidColorBrush(Colors.Red),
                                StrokeThickness = 4
                            };
                            routeLine.Locations = new LocationCollection();

                            foreach (var loc in feature.value.geometry.coordinates)
                            {
                                routeLine.Locations.Add(new Location(loc[1], loc[0]));
                            }

                            mapPolylines.Add(routeLine);

                            Pushpin pin = new Pushpin()
                            {
                                Location = new Location(feature.value.geometry.coordinates[0][1],
                                feature.value.geometry.coordinates[0][0]),
                                ToolTip = "Departure",
                            };
                            pins.Add(pin);
                            break;
                        case 1:

                            MapPolyline routeLine2 = new MapPolyline()
                            {
                                Stroke = new SolidColorBrush(Colors.Yellow),
                                StrokeThickness = 4
                            };
                            routeLine2.Locations = new LocationCollection();

                            foreach (var loc in feature.value.geometry.coordinates)
                            {
                                routeLine2.Locations.Add(new Location(loc[1], loc[0]));
                            }

                            mapPolylines.Add(routeLine2);

                            Pushpin pin1 = new Pushpin()
                            {
                                Location = new Location(feature.value.geometry.coordinates[0][1],
                                feature.value.geometry.coordinates[0][0]),
                                ToolTip = "Departure Station"
                            };
                            pins.Add(pin1);
                            break;
                        case 2:

                            MapPolyline routeLine3 = new MapPolyline()
                            {
                                Stroke = new SolidColorBrush(Colors.LightGreen),
                                StrokeThickness = 4
                            };
                            routeLine3.Locations = new LocationCollection();

                            foreach (var loc in feature.value.geometry.coordinates)
                            {
                                routeLine3.Locations.Add(new Location(loc[1], loc[0]));
                            }

                            mapPolylines.Add(routeLine3);

                            Pushpin pin2 = new Pushpin()
                            {
                                Location = new Location(feature.value.geometry.coordinates[0][1],
                                feature.value.geometry.coordinates[0][0]),
                                ToolTip = "Arrival Station"
                            };
                            pins.Add(pin2);
                            break;
                    }

                    foreach (var coordinate in feature.value.geometry.coordinates)
                    {
                        locs.Add(new Location(coordinate[1], coordinate[0]));
                    }
                }
            }

            //Adding pins and routes with different colours
            for (int i = 0; i < mapPolylines.Count; i++)
            {
                MyMap.Children.Add(mapPolylines[i]);
            }

            for (int i = 0; i < pins.Count; i++)
            {
                MyMap.Children.Add(pins[i]);
            }
            //Adding last pin
            Pushpin pinFinal = new Pushpin()
            {
                Location = locs[locs.Count - 1],
                ToolTip = "Arrival"
            };

            MyMap.Children.Add(pinFinal);
        }

        private async void DetailsSetup()
        {
            double dur = 0, dist = 0;
            //Set Steps
            foreach (var element in this.geoJsons)
            {
                foreach (var segment in element.features[0].properties.segments)
                {
                    foreach (var step in segment.steps)
                    {
                        directions.Items.Add(new ListBoxItem()
                        {
                            Content = step.instruction
                        });
                    }

                    dur += segment.duration;
                    dist += segment.distance;
                }
            }

            Distance.Content = (dist / 1000).ToString() + "km";
            Duration.Content = (dur / 3600) + "h";

            int lastSize = this.geoJsons[this.geoJsons.Length - 1].features[0].properties.segments[0].steps.Length - 1;
            DepartAdress.Content = this.geoJsons[0].features[0].properties.segments[0].steps[0].name;
            ArriveAdress.Content = this.geoJsons[this.geoJsons.Length - 1].features[0]
                .properties.segments[0].steps[lastSize - 1].name;

            mostVDeparture.Content = await GetMostUsedDepStation();
            mostVArrival.Content = await GetMostUsedArrStation();

            var stations = await GetFiveMostUsed();

            SeriesCollection seriesCollection = new SeriesCollection();

            foreach(var station in stations)
            {
                seriesCollection.Add(new ColumnSeries
                {
                    Title = station.Key,
                    Values = new ChartValues<int> { station.Value },
                    Fill = Brushes.Blue,
                });
            }
            chart.Series.AddRange(seriesCollection);
        }

        private async Task<string> GetMostUsedDepStation()
        {
            CollectionReference stationsDeparture = database.Collection("StationsDeparture");
            QuerySnapshot snapshot = await stationsDeparture.GetSnapshotAsync();

            int max = 0;
            string id = "";
            foreach (var doc in snapshot)
            {
                StationStatistics current = doc.ConvertTo<StationStatistics>();
                if (current.occurence > max)
                {
                    max = current.occurence;
                    id = current.station.number.ToString();
                }
            }

            DocumentSnapshot foundSnapchot = await stationsDeparture.Document(id).GetSnapshotAsync();
            StationStatistics found = foundSnapchot.ConvertTo<StationStatistics>();

            return found.occurence + "x -->" + found.station.name + "," + found.station.contractName;
        }

        private async Task<string> GetMostUsedArrStation()
        {
            CollectionReference stationsArrival = database.Collection("StationsArrival");
            QuerySnapshot snapshot = await stationsArrival.GetSnapshotAsync();

            int max = 0;
            string id = "";
            foreach (var doc in snapshot)
            {
                StationStatistics current = doc.ConvertTo<StationStatistics>();
                if (current.occurence > max)
                {
                    max = current.occurence;
                    id = current.station.number.ToString();
                }
            }

            DocumentSnapshot foundSnapchot = await stationsArrival.Document(id).GetSnapshotAsync();
            StationStatistics found = foundSnapchot.ConvertTo<StationStatistics>();

            return found.occurence + "x -->" + found.station.name + "," + found.station.contractName;
        }

        private async Task<Dictionary<string, int>> GetFiveMostUsed()
        {
            Dictionary<string, int> pairs = new Dictionary<string, int>();

            CollectionReference stationsDeparture = database.Collection("StationsDeparture");
            QuerySnapshot snapshotDeparture = await stationsDeparture.GetSnapshotAsync();

            CollectionReference stationsArrival = database.Collection("StationsArrival");
            QuerySnapshot snapshotArrival = await stationsArrival.GetSnapshotAsync();

            var documents = snapshotDeparture.Documents.ToList().Union(snapshotArrival.Documents.ToList()).OrderByDescending(x => x.ConvertTo<StationStatistics>().occurence);

            foreach (var doc in documents.Take(5))
            {
                StationStatistics currentStation = doc.ConvertTo<StationStatistics>();
                pairs.Add(currentStation.station.name + "\n[" + currentStation.station.contractName + "]" , currentStation.occurence);
            }

            return pairs;
        }
    }
}