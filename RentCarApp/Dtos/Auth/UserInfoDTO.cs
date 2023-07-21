namespace RentCarApp.Dtos.Auth
{
	public class UserInfoDTO
	{
		public string Email { get; set; }
		public string Token { get; set; }
		public bool? EmailVerified { get; set; }
		public bool? IpVerified { get; set; }

	}
}
