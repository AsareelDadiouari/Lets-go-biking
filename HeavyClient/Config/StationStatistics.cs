using Google.Cloud.Firestore;

namespace HeavyClient.Config
{
    [FirestoreData]
    public class StationStatistics
    {
        public enum TypeStation
        {
            DEFAULT,
            DEPARTURE,
            ARRIVAL
        }

        [FirestoreProperty] public Station station { get; set; }

        [FirestoreProperty] public int occurence { get; set; }

        [FirestoreProperty] public TypeStation type { get; set; }
    }
}