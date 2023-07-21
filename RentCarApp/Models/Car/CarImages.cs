namespace RentCarApp.Models.Car
{
	public class CarImages
	{
		public int Id { get; set; }
		public string ImageUrl { get; set; }

		//relation		
		public Cars Cars { get; set; }
	}
}
