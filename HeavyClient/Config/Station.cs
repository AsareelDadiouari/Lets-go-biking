using Google.Cloud.Firestore;

namespace HeavyClient.Config
{
    [FirestoreData]
    public class Station
    {
        [FirestoreProperty]
        public int number { get; set; }
        [FirestoreProperty]
        public string contractName { get; set; }
        [FirestoreProperty]
        public string name { get; set; }
        [FirestoreProperty]
        public string address { get; set; }
    }
}
