using HeavyClient.Routing;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Microsoft.Toolkit.Wpf.UI.Controls;
using System;
using System.Collections.Generic;
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
using Microsoft.Maps.MapControl.WPF;
using System.IO;

namespace HeavyClient.Data.ViewModels
{
    /// <summary>
    /// Interaction logic for Map.xaml
    /// </summary>
    public partial class Map : Page
    {
        GeoGeoJson[] geoJsons;
        public Map(GeoGeoJson[] geoJsons)
        {
            InitializeComponent();
            this.geoJsons = geoJsons;
            //Setting default view
            MyMap.SetView(new Location(this.geoJsons[0].features[0].geometry.coordinates[0][1],
                this.geoJsons[0].features[0].geometry.coordinates[0][0]), 15);
            //MyMap.Mode = new AerialMode(false);
            Setup();
        }

        private void Setup()
        {
            LocationCollection locs = new LocationCollection();
            List<MapPolyline> mapPolylines = new List<MapPolyline>();
            List<Pushpin> pins = new List<Pushpin>();  

            foreach (var data in this.geoJsons.Select((value, index) => new { value, index }))
            {
                foreach (var feature in data.value.features)
                {
                    foreach (var coordinate in feature.geometry.coordinates)
                    {
                        locs.Add(new Location(coordinate[1], coordinate[0]));
                    }
                }

                switch(data.index)
                {
                    case 0:
                        MapPolyline routeLine = new MapPolyline()
                        {
                            Locations = locs,
                            Stroke = new SolidColorBrush(Colors.Red),
                            StrokeThickness = 5
                        };
                        Pushpin pin = new Pushpin()
                        {
                            Location = locs[data.index]
                        };

                        pins.Add(pin);
                        mapPolylines.Add(routeLine);
                        locs.Clear();
                        break;

                    case 1:
                        MapPolyline routeLine2 = new MapPolyline()
                        {
                            Locations = locs,
                            Stroke = new SolidColorBrush(Colors.Yellow),
                            StrokeThickness = 5
                        };
                        Pushpin pin2 = new Pushpin()
                        {
                            Location = locs[0]
                        };

                        pins.Add(pin2);
                        mapPolylines.Add(routeLine2);
                        break;

                    case 2:
                        MapPolyline routeLine3 = new MapPolyline()
                        {
                            Locations = locs,
                            Stroke = new SolidColorBrush(Colors.Green),
                            StrokeThickness = 5
                        };
                        Pushpin pin3 = new Pushpin()
                        {
                            Location = locs[0]
                        };

                        pins.Add(pin3);
                        mapPolylines.Add(routeLine3);
                        break;
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
                Location = locs.Count - 1 > 0 ? locs[locs.Count - 1] : locs[0]
            };

            MyMap.Children.Add(pinFinal);
        }

    }
}
