namespace RentCarApp.Dtos.Cars
{
	public class FilterDTO
	{
		public string? City { get; set; }
		public string? Brand { get; set; }
		public int? StartYear { get; set; }
		public int? EndYear { get; set; }
		public int? Capacity { get; set; }
		public int? MinPrice { get; set; }
		public int? MaxPrice { get; set; }

	}
}
