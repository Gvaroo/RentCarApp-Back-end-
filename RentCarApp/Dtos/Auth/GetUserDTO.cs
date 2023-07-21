namespace RentCarApp.Dtos.Auth
{
	public class GetUserDTO
	{
		public string Name { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public int Number { get; set; }
		public int Amount { get; set; }
		public bool IsVerified { get; set; }
	}
}
