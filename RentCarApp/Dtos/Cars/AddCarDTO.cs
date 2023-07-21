using RentCarApp.Models.Car;

namespace RentCarApp.Dtos.Cars
{
	public class AddCarDTO
	{
		public string Name { get; set; }
		public int Year { get; set; }
		public int Price { get; set; }
		public string City { get; set; }
		public int Capacity { get; set; }
		public int FuelCapacity { get; set; }
		public string CreatedBy { get; set; }
		public int BrandId { get; set; }
		public int ModelId { get; set; }
		public int TransmissionId { get; set; }
		public List<string> Images { get; set; }
	}
}
