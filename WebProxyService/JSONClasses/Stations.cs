using System;
using System.Collections.Generic;

namespace WebProxyService.JSONClasses
{
    public class Position
    {
        public Position(double lat, double lon)
        {
            latitude = lat;
            longitude = lon;
        }

        public Position()
        {
        }

        public double latitude { get; set; }
        public double longitude { get; set; }

        public double getDistanceTo(Position other)
        {
            /*var d1 = latitude * (Math.PI / 180.0);
            var num1 = longitude * (Math.PI / 180.0);
            var d2 = other.latitude * (Math.PI / 180.0);
            var num2 = other.longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));*/
            return Math.Sqrt(Math.Pow(other.longitude - longitude, 2) + Math.Pow(other.latitude - latitude, 2));
        }

        public override bool Equals(object obj)
        {
            var other = (Position) obj;
            return latitude == other.latitude && longitude == other.longitude;
        }

        public override int GetHashCode()
        {
            return (latitude, longitude).GetHashCode();
        }
    }

    public class Availabilities
    {
        public int bikes { get; set; }
        public int stands { get; set; }
        public int mechanicalBikes { get; set; }
        public int electricalBikes { get; set; }
        public int electricalInternalBatteryBikes { get; set; }
        public int electricalRemovableBatteryBikes { get; set; }
    }

    public class TotalStands
    {
        public Availabilities availabilities { get; set; }
        public int capacity { get; set; }
    }

    public class MainStands
    {
        public Availabilities availabilities { get; set; }
        public int capacity { get; set; }
    }

    public class OverflowStands
    {
        public Availabilities availabilities { get; set; }
        public int capacity { get; set; }
    }

    public class Station
    {
        public int number { get; set; }
        public string contractName { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public Position position { get; set; }
        public bool banking { get; set; }
        public bool bonus { get; set; }
        public string status { get; set; }
        public DateTime? lastUpdate { get; set; }
        public bool connected { get; set; }
        public bool overflow { get; set; }
        public object shape { get; set; }
        public TotalStands totalStands { get; set; }
        public MainStands mainStands { get; set; }
        public OverflowStands overflowStands { get; set; }

        public override bool Equals(object obj)
        {
            var other = (Station) obj;
            return number == other.number && name.Equals(other.name) && contractName.Equals(other.contractName);
        }

        public override int GetHashCode()
        {
            return (number, contractName, address, name).GetHashCode();
        }
    }

    public class Stations
    {
        public List<Station> stations { get; set; }
    }
}