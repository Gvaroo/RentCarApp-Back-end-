namespace RentCarApp.Models.user
{
	public class Notification
	{
		public int Id { get; set; }
		public string Message { get; set; }
		public bool IsRead { get; set; } = false;

		//relation

		public User.User User { get; set; }
		public int UserId { get; set; }
	}
}
