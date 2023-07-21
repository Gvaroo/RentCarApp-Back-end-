namespace RentCarApp.Dtos.Cars
{
	public class RentCarDTO
	{
		public string UserEmail { get; set; }
		public int CarId { get; set; }
		public int RentDays { get; set; }
		public int Price { get; set; }
	}
}
