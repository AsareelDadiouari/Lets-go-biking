using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HeavyClient.Routing;

namespace HeavyClient.Data.ViewModels
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        Service1Client service;
        public MainMenu()
        {
            service = new Service1Client();
            InitializeComponent();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            GeoGeoJson[] geoJsons = service.GetGeoData(departure.Text, arrival.Text);
            Map mapPage = new Map(geoJsons);
            this.NavigationService.Navigate(mapPage);
        }
    }
}
