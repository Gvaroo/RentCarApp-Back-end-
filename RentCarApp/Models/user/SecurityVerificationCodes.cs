namespace RentCarApp.Models.user
{
	public class SecurityVerificationCodes
	{
		public int Id { get; set; }
		public string VerificationCode { get; set; }
		public DateTime ExpireDate { get; set; }

		//Relation

		public User.User User { get; set; }
	}
}
