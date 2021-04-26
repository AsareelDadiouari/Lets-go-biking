using System.Windows;
using System.Windows.Controls;
using HeavyClient.Routing;

namespace HeavyClient.Data.ViewModels
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        private Service1Client service;
        public MainMenu()
        {
            service = new Service1Client();
            InitializeComponent();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            GeoGeoJson[] geoJsons = service.GetGeoData(departure.Text, arrival.Text);
            Statistics stats = service.GetStatistics();

            if (geoJsons.Length == 0)
            {
                MessageBoxResult result = MessageBox.Show("No Adress was found",
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                if (result.Equals(MessageBoxButton.OK))
                {
                    this.Focus();
                }
            } else
            {
                Map mapPage = new Map(geoJsons, stats);
                this.NavigationService.Navigate(mapPage);
            }

            //service.Save();
        }
    }
}
