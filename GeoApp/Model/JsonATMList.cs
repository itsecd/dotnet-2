using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GeoApp.Model
{
    public class JsonATM
    {
        [JsonPropertyName("geometry")]
        public Geometry Geometry { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        public override bool Equals(object o)
        {
            return o is JsonATM aTM &&
                aTM.Geometry.Coordinates[0] == Geometry.Coordinates[0] &&
                aTM.Geometry.Coordinates[1] == Geometry.Coordinates[1] &&
                aTM.Properties.Id == Properties.Id &&
                aTM.Properties.Operator == Properties.Operator &&
                aTM.Properties.Balance == Properties.Balance;
        }
    }

    public class Geometry
    {
        [JsonPropertyName("coordinates")]
        public List<double> Coordinates { get; set; }
    }

    public class Properties
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("operator")]
        public string Operator { get; set; }

        [JsonPropertyName("balance")]
        public int Balance { get; set; }
    }

    public class JsonATMList
    {
        [JsonPropertyName("features")]
        public List<JsonATM> ATMs { get; set; }
    }
}
