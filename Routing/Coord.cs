using System.Globalization;
using System.Threading.Tasks;
using WebProxyService.JSONClasses;

namespace Routing
{
    internal class Coord
    {
        public Coord(Task<string> position)
        {
            pos = position.Result;
            var arr = pos.Split(',');
            longitude = arr[0];
            latitude = arr[1];
        }

        public string longitude { get; set; }
        public string latitude { get; set; }
        public string pos { get; set; }

        public Position ToPosition()
        {
            var position = new Position();
            position.latitude = double.Parse(latitude, CultureInfo.InvariantCulture);
            position.longitude = double.Parse(longitude, CultureInfo.InvariantCulture);
            return position;
        }

        public override string ToString()
        {
            var toReturn = "";

            toReturn = "-------Coord-----\n";
            toReturn += pos + "\n" + "lon=" + longitude + "\nlat=" + latitude + "\n";
            toReturn += "-------Coord-----\n";

            return toReturn;
        }
    }
}