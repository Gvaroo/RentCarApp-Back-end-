namespace RentCarApp.Dtos.Notifications
{
	public class SendRentNotificationDTO
	{
		public int CarId { get; set; }
		public string UserEmail { get; set; }
		public int RentDays { get; set; }
	}
}
