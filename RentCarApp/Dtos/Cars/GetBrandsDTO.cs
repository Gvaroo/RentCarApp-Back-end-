using RentCarApp.Models.Car;

namespace RentCarApp.Dtos.Cars
{
	public class GetBrandsDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<GetModelDTO> models { get; set; }

	}
}
