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
                            
                            foreach(var loc in feature.value.geometry.coordinates)
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

        private void DetailsSetup()
        {
            detailsItem.Content = "Hum Charal !";
        }
    }
}
