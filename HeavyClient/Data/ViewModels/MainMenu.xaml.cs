using Google.Cloud.Firestore;
using HeavyClient.Config;
using Routing;
using System;
using System.Windows;
using System.Windows.Controls;
using static HeavyClient.Config.StationStatistics;
using Station = Routing.Station;

namespace HeavyClient.Data.ViewModels
{
    /// <summary>
    ///     Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        private readonly string configURL;
        private readonly FirestoreDb database;
        private readonly Service1Client service;

        public MainMenu()
        {
            InitializeComponent();
            service = new Service1Client();
            configURL = AppDomain.CurrentDomain.BaseDirectory + "\\config.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", configURL);
            database = FirestoreDb.Create("let-s-go-biking");
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            Search.IsEnabled = false;
            var geoJsons = await service.GetGeoDataAsync(departure.Text, arrival.Text);
            Search.IsEnabled = true;
            AddStation(geoJsons[0].station, TypeStation.DEPARTURE);
            AddStation(geoJsons[geoJsons.Length - 1].station, TypeStation.ARRIVAL);

            MainWindow.routeSearches.Add(departure.Text + "-" + arrival.Text + "-");

            if (geoJsons.Length == 0)
            {
                var result = MessageBox.Show("No Adress was found",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                if (result.Equals(MessageBoxButton.OK)) Focus();
            }
            else
            {
                var mapPage = new Map(geoJsons);
                NavigationService.Navigate(mapPage);
            }
        }

        private async void AddStation(Station sta, TypeStation typeStation)
        {
            var stations = database.Collection("Stations");

            if (typeStation.Equals(TypeStation.DEPARTURE))
                stations = database.Collection("StationsDeparture");
            else if (typeStation.Equals(TypeStation.ARRIVAL)) stations = database.Collection("StationsArrival");

            if (sta != null)
                try
                {
                    var stationRef = stations.Document(sta.number.ToString());
                    await stationRef.UpdateAsync("occurence", FieldValue.Increment(1));
                }
                catch (Exception e)
                {
                    var stationStatistics = new StationStatistics
                    {
                        type = typeStation,
                        station = new Config.Station
                        {
                            address = sta.address,
                            contractName = sta.contractName,
                            name = sta.name,
                            number = sta.number
                        },
                        occurence = 0
                    };
                    await stations.Document(sta.number.ToString()).SetAsync(stationStatistics);
                }
        }
    }
}