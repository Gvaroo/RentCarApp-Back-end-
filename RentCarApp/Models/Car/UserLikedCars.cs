using RentCarApp.Models.User;
using System.ComponentModel.DataAnnotations;

namespace RentCarApp.Models.Car
{
	public class UserLikedCar
	{
		public int UserId { get; set; }
		public User.User User { get; set; }

		public int CarId { get; set; }
		public Cars Car { get; set; }
	}
}
