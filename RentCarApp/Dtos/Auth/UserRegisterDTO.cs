using System.ComponentModel.DataAnnotations;

namespace RentCarApp.Dtos.Auth
{
	public class UserRegisterDTO
	{
		public string Name { get; set; }
		public string LastName { get; set; }
		public string Password { get; set; }
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		public int Number { get; set; }
	}
}
