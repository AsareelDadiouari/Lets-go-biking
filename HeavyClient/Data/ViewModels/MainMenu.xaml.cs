using Google.Cloud.Firestore;
using HeavyClient.Config;
using Routing;
using System;
using System.Windows;
using System.Windows.Controls;
using static HeavyClient.Config.StationStatistics;

namespace HeavyClient.Data.ViewModels
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        private Service1Client service;
        FirestoreDb database;
        readonly private string configURL = AppDomain.CurrentDomain.BaseDirectory + "\\config.json";
        public MainMenu()
        {
            service = new Service1Client();
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", configURL);
            database = FirestoreDb.Create("let-s-go-biking");
            InitializeComponent();
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            GeoGeoJson[] geoJsons = await service.GetGeoDataAsync(departure.Text, arrival.Text);
            AddStation(geoJsons[0].station, StationStatistics.TypeStation.DEPARTURE);
            AddStation(geoJsons[geoJsons.Length - 1].station, StationStatistics.TypeStation.ARRIVAL);

            if (geoJsons.Length == 0)
            {
                MessageBoxResult result = MessageBox.Show("No Adress was found",
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                if (result.Equals(MessageBoxButton.OK))
                {
                    this.Focus();
                }
            }
            else
            {
                Map mapPage = new Map(geoJsons);
                this.NavigationService.Navigate(mapPage);
            }
        }

        private async void AddStation(Routing.Station sta, StationStatistics.TypeStation typeStation)
        {
            CollectionReference stations = database.Collection("Stations");

            if (typeStation.Equals(TypeStation.DEPARTURE))
            {
                stations = database.Collection("StationsDeparture");
            }
            else if (typeStation.Equals(TypeStation.ARRIVAL))
            {
                stations = database.Collection("StationsArrival");
            }

            if (sta != null)
            {
                try
                {
                    DocumentReference stationRef = stations.Document(sta.number.ToString());
                    await stationRef.UpdateAsync("occurence", FieldValue.Increment(1));
                }
                catch (Exception e)
                {
                    StationStatistics stationStatistics = new StationStatistics()
                    {
                        type = typeStation,
                        station = new Config.Station()
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
}
