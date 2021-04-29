using System;
using System.Collections.Generic;

namespace WebProxyService.JSONClasses
{
    public class Parameters
    {
        public string dataset { get; set; }
        public string timezone { get; set; }
        public int rows { get; set; }
        public int start { get; set; }
        public string format { get; set; }
        public List<string> facet { get; set; }
    }

    public class Fields
    {
        public string status { get; set; }
        public string contract_name { get; set; }
        public string name { get; set; }
        public string bonus { get; set; }
        public int bike_stands { get; set; }
        public int number { get; set; }
        public DateTime last_update { get; set; }
        public int available_bike_stands { get; set; }
        public string banking { get; set; }
        public int available_bikes { get; set; }
        public string address { get; set; }
        public List<double> position { get; set; }
    }

    public class Geometry
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }

    public class Record
    {
        public string datasetid { get; set; }
        public string recordid { get; set; }
        public Fields fields { get; set; }
        public Geometry geometry { get; set; }
        public DateTime? record_timestamp { get; set; }
    }

    public class Facet
    {
        public int count { get; set; }
        public string path { get; set; }
        public string state { get; set; }
        public string name { get; set; }
    }

    public class FacetGroup
    {
        public List<Facet> facets { get; set; }
        public string name { get; set; }
    }

    public class Bikes
    {
        public int nhits { get; set; }
        public Parameters parameters { get; set; }
        public List<Record> records { get; set; }
        public List<FacetGroup> facet_groups { get; set; }
    }
}