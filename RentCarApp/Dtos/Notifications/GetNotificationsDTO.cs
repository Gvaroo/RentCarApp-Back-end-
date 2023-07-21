namespace RentCarApp.Dtos.Notifications
{
	public class GetNotificationsDTO
	{
		public int Id { get; set; }
		public string Message { get; set; }
		public bool IsRead { get; set; }
	}
}
