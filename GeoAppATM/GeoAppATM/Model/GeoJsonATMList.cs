using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GeoAppATM.Model
{

    /// <summary>
    /// Data type for reading ATMs from geojson
    /// </summary>
    public class GeoJsonATMList
    {
        [JsonPropertyName("features")]
        public List<GeoJsonATM> ATMs { get; set; }
    }
    /// <summary>
    /// A class containing information about an ATM
    /// </summary>
    public class GeoJsonATM
    {
        /// <summary>
        /// Contains the coordinates of the ATM
        /// </summary>
        [JsonPropertyName("geometry")]
        public Geometry Geometry { get; set; }

        /// <summary>
        /// Contains the ID, operator name, balance of the ATM
        /// </summary>
        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        /// <summary>
        /// Equal ATMs
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is GeoJsonATM atm &&
                atm.Geometry.Coordinates[0] == Geometry.Coordinates[0] &&
                atm.Geometry.Coordinates[1] == Geometry.Coordinates[1] &&
                atm.Properties.Id == Properties.Id &&
                atm.Properties.Operator == Properties.Operator &&
                atm.Properties.Balance == Properties.Balance;
        }

        /// <summary>
        /// Hash code ATM by ID
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            try
            {
                return int.Parse(Properties.Id);
            }
            catch
            {
                return 0;
            }
        }
    }

    /// <summary>
    /// A class for storing ATM coordinates
    /// </summary>
    public class Geometry
    {
        [JsonPropertyName("coordinates")]
        public List<double> Coordinates { get; set; }
    }

    /// <summary>
    /// A class for storing ATM ID, operator name, balance
    /// </summary>
    public class Properties
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("operator")]
        public string Operator { get; set; }

        [JsonPropertyName("balance")]
        public int Balance { get; set; }
    }
}
