using Microsoft.EntityFrameworkCore;
using RentCarApp.Models.Car;
using RentCarApp.Models.user;
using RentCarApp.Models.User;

namespace RentCarApp.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
		}
		public DbSet<User> users { get; set; }
		public DbSet<Cars> cars { get; set; }
		public DbSet<Brand> Brands { get; set; }
		public DbSet<CarModel> carModels { get; set; }
		public DbSet<Transmission> Transmissions { get; set; }
		public DbSet<CarImages> CarImages { get; set; }
		public DbSet<UserLikedCar> UserLikedCar { get; set; }
		public DbSet<RentedCars> RentedCars { get; set; }
		public DbSet<Notification> Notification { get; set; }
		public DbSet<SecurityVerificationCodes> SecurityVerificationCodes { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UserLikedCar>()
				.HasKey(ulc => new { ulc.UserId, ulc.CarId });

			modelBuilder.Entity<UserLikedCar>()
				.HasOne(ulc => ulc.User)
				.WithMany(u => u.UserLikedCars)
				.HasForeignKey(ulc => ulc.UserId);

			modelBuilder.Entity<UserLikedCar>()
				.HasOne(ulc => ulc.Car)
				.WithMany(c => c.UserLikedCars)
				.HasForeignKey(ulc => ulc.CarId);
		}
	}
}
