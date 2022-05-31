namespace GeoAppATM.Model
{
	/// <summary>
	/// A class containing information about an ATM
	/// </summary>

	public class Atm
    {
		/// <summary>
		/// ID
		/// </summary>
		
		public string Id { get; set; }

		/// <summary>
		/// Operator name
		/// </summary>
		
		public string Name { get; set; }

		/// <summary>
		/// Latitude: coordinate X
		/// </summary>

		public double Latitude { get; set; }

		/// <summary>
		/// Longitude: coordinate Y
		/// </summary>

		public double Longitude { get; set; }

		/// <summary>
		/// Balance
		/// </summary>

		public int Balance { get; set; }
	}
}
