using System.Threading.Tasks;

namespace Routing
{
    class Coord
    {
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string pos { get; set; }
        public Coord(Task<string> posititon)
        {
            this.pos = posititon.Result; 
            string[] arr = this.pos.Split(',');
            longitude = arr[0];
            latitude = arr[1];
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
