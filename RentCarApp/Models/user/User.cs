using RentCarApp.Models.Car;
using RentCarApp.Models.user;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RentCarApp.Models.User
{
	public class User
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string LastName { get; set; }
		[Required]
		public string PasswordHash { get; set; }
		[Required]
		public string PasswordSalt { get; set; }
		[DataType(DataType.EmailAddress)]
		[Required]
		public string Email { get; set; }
		[Required]
		public int Number { get; set; }

		public int Amount { get; set; }
		public bool IsVerified { get; set; } = false;
		public string? LastLoginIpAddress { get; set; }

		//relation
		public List<Cars>? Cars { get; set; } = new List<Cars>(); //Many-To-One relationship with car
		public List<UserLikedCar>? UserLikedCars { get; set; } = new List<UserLikedCar>(); //Many-To-One relationship with UserLikedCars
		public List<RentedCars>? RentedCars { get; set; } = new List<RentedCars>(); //Many-To-One relationship with RentedCars
		public List<Notification>? Notifications { get; set; } = new List<Notification>(); //Many-To-One relationship with Notification
		public List<SecurityVerificationCodes>? SecurityVerificationCodes { get; set; } = new List<SecurityVerificationCodes>(); //Many-To-One relationship with SecurityVerificationCodes
	}
}
