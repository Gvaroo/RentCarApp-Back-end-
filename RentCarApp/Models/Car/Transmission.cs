namespace RentCarApp.Models.Car
{
	public class Transmission
	{
		public int Id { get; set; }
		public string Name { get; set; }

		//Relation
		public List<Cars>? Car { get; set; } = new List<Cars>();

	}
}
