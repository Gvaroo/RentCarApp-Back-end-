namespace RentCarApp.Models.Car
{
	public class RentedCars
	{
		public int Id { get; set; }
		public User.User User { get; set; }
		public Cars Car { get; set; }
		public int CarId { get; set; }
		public int RentDays { get; set; }
		public int Price { get; set; }
		public DateTime RentEndDate { get; set; }
		public DateTime LastUpdated { get; set; }
	}
}
