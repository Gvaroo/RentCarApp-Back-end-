using RentCarApp.Models.Car;

namespace RentCarApp.Dtos.Cars
{
	public class GetCarDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Year { get; set; }
		public int Price { get; set; }
		public string City { get; set; }
		public int Capacity { get; set; }
		public int FuelCapacity { get; set; }
		public GetBrandDTO Brand { get; set; }
		public GetModelDTO Model { get; set; }
		public GetTransmissionsDTO Transmission { get; set; }
		public List<GetImagesDTO> CarImages { get; set; }
		public string UserEmail { get; set; }
		public bool IsRented { get; set; }
	}
}
