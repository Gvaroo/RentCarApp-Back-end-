namespace RentCarApp.Models.Car
{
	public class CarModel
	{
		public int Id { get; set; }
		public string Name { get; set; }

		//relations
		public Brand? Brand { get; set; }
		public List<Cars>? Car { get; set; } = new List<Cars>();


	}
}
