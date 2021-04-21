using System;
using System.Globalization;
using System.Threading.Tasks;
using WebProxyService.JSONClasses;

namespace Routing
{
    class Coord
    {
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string pos { get; set; }
        public Coord(Task<string> position)
        {
            this.pos = position.Result; 
            string[] arr = this.pos.Split(',');
            this.longitude = arr[0];
            this.latitude = arr[1];
        }

        public Position ToPosition()
        {
            Position position = new Position();
            position.latitude = double.Parse(latitude, CultureInfo.InvariantCulture);
            position.longitude = double.Parse(longitude, CultureInfo.InvariantCulture);
            return position;
        }

        public override string ToString()
        {
            string toReturn = "";

            toReturn = "-------Coord-----\n";
            toReturn += this.pos + "\n" + "lon=" + longitude + "\nlat=" + latitude + "\n";
            toReturn += "-------Coord-----\n";

            return toReturn;
        }
    }
}
