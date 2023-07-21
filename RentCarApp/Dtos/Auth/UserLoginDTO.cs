using System.ComponentModel.DataAnnotations;

namespace RentCarApp.Dtos.Auth
{
	public class UserLoginDTO
	{
		[DataType(DataType.EmailAddress)]
		[Required]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
	}
}
