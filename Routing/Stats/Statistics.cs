using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using WebProxyService.JSONClasses;

namespace Routing.Stats
{
    public class Statistics
    {
        public HashSet<StationsStatistics> stationsStats;
        public Statistics()
        {
            stationsStats = new HashSet<StationsStatistics>();
            string url = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + AppDomain.CurrentDomain.BaseDirectory + "\\stats.accdb;PERSIST SECURITY INFO=False";

            using (OleDbConnection connection = new OleDbConnection(url))
            {
                string query = "SELECT * FROM Station_Statistics";
                OleDbCommand command = new OleDbCommand(query);
                command.Connection = connection;

                try
                {
                    connection.Open();
                    var s = command.CommandText;
                    Console.WriteLine("DataSource: {0} \nDatabase: {1}",
                connection.DataSource, connection.Database);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public enum Type
        {
            DEFAULT,
            DEPARTURE,
            ARRIVAL
        }

        [Serializable]
        public class StationsStatistics
        {
            [JsonProperty("station")]
            public Station station;
            [JsonProperty("occurence")]
            public int occurence { get; set; }
            [JsonProperty("type")]
            public Type type;

            public override bool Equals(object obj)
            {
                StationsStatistics other = (StationsStatistics)obj;
                return this.station.Equals(other.station) && this.type.Equals(other.type);
            }
        }

        public Station MostVisitedDepartureStation()
        {
            var newlist = stationsStats.OrderByDescending(x => x.type == Type.DEPARTURE).ThenBy(x => x.occurence).ToList();
            return newlist[0].station;
        }

        public Station MostVisitedArrivalStation()
        {
            var newlist = stationsStats.OrderByDescending(x => x.type == Type.ARRIVAL).ThenBy(x => x.occurence).ToList();
            return newlist[0].station;
        }

        /**
         * List the 5 most visited stations and their occurence 
         * */
        public List<StationsStatistics> MostVisitedStations()
        {
            var newlist = stationsStats.OrderByDescending(x => x.occurence).ToList().Take(5);
            return (List<StationsStatistics>)newlist;
        }

        public void Set(Station other, Type type)
        {
            var sta = stationsStats.Where(x => x.station == other && x.type == type).FirstOrDefault();
            if (sta != null)
            {
                stationsStats.Remove(sta);
                sta.occurence++;
                stationsStats.Add(sta);
            } else
            {
                stationsStats.Add(new StationsStatistics
                {
                    occurence = 0,
                    station = other,
                    type = type
                });
                var s = stationsStats;
            }
        }

        public void Persist()
        {
        }
    }
}
