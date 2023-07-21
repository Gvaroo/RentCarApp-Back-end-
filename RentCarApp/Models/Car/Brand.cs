namespace RentCarApp.Models.Car
{
	public class Brand
	{
		public int Id { get; set; }
		public string Name { get; set; }

		//relations
		public List<Cars>? Car { get; set; } = new List<Cars>();  //Many-To-One relationship with Car
		public List<CarModel>? models { get; set; } = new List<CarModel>();  //Many-To-one relationship with CarModel
	}
}
