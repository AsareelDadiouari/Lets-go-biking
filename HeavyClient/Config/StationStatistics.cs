using Google.Cloud.Firestore;

namespace HeavyClient.Config
{
    [FirestoreData]
    public class StationStatistics
    {
        [FirestoreProperty]
        public Station station { get; set; }
        [FirestoreProperty]
        public int occurence { get; set; }
        public enum TypeStation
        {
            DEFAULT,
            DEPARTURE,
            ARRIVAL
        }
        [FirestoreProperty]
        public TypeStation type { get; set; }
    }
}
