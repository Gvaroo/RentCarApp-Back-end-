using RentCarApp.Models.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentCarApp.Models.Car
{
	public class Cars
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Year { get; set; }
		public int Price { get; set; }
		public string City { get; set; }
		public int Capacity { get; set; }
		public int FuelCapacity { get; set; }
		public int? Latitude { get; set; }
		public int? longtitude { get; set; }
		public int Likes { get; set; } = 0;
		public bool IsRented { get; set; } = false;

		[NotMapped]
		public string CreatedBy { get; set; }

		//relations
		public User.User User { get; set; } //One-To-Many relationship with car
		public Brand Brand { get; set; }  //One-To-Many relationship with Brand
		public int BrandId { get; set; }
		public CarModel? Model { get; set; }  //One-To-Many relationship with CarModel
		public int ModelId { get; set; }
		public Transmission Transmission { get; set; }  //One-To-many relationship with Transmission
		public int TransmissionId { get; set; }
		public List<CarImages> CarImages { get; set; } = new List<CarImages>();
		public List<UserLikedCar> UserLikedCars { get; set; } = new List<UserLikedCar>();  //One-To-Many relationship with UserLikedCars
		public List<RentedCars>? RentedCar { get; set; } = new List<RentedCars>(); //Many-To-One relationship with RentedCars


	}
}
