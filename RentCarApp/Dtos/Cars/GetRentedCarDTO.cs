using RentCarApp.Models.Car;

namespace RentCarApp.Dtos.Cars
{
	public class GetRentedCarDTO
	{
		public string Name { get; set; }
		public string City { get; set; }
		public GetBrandDTO Brand { get; set; }
		public GetModelDTO Model { get; set; }
		public RentCarDTO RentedCar { get; set; }


	}
}
