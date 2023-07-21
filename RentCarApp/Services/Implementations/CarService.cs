using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Hangfire;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RentCarApp.Data;
using RentCarApp.Dtos.Cars;
using RentCarApp.Models;
using RentCarApp.Models.Car;
using RentCarApp.Services.Interfaces;
using System.Text.RegularExpressions;


namespace RentCarApp.Services.Implementations
{
	public class CarService : ICarService
	{
		private readonly ApplicationDbContext _db;
		private readonly IMapper _mapper;
		private readonly IConfiguration _configuration;


		public CarService(ApplicationDbContext db, IMapper mapper, IConfiguration configuration)
		{
			_db = db;
			_mapper = mapper;
			_configuration = configuration;
		}

		public async Task<ServiceResponse<GetCarDTO>> AddCar(AddCarDTO newCar)
		{
			var response = new ServiceResponse<GetCarDTO>();
			try
			{
				var brand = await _db.Brands.FirstOrDefaultAsync(c => c.Id == newCar.BrandId);
				var model = await _db.carModels.FirstOrDefaultAsync(c => c.Id == newCar.ModelId);
				var transmission = await _db.Transmissions.FirstOrDefaultAsync(c => c.Id == newCar.TransmissionId);
				var user = await _db.users.FirstOrDefaultAsync(c => c.Email == newCar.CreatedBy);
				var car = _mapper.Map<Cars>(newCar);
				car.User = user;
				car.Brand = brand;
				car.Model = model;
				car.Transmission = transmission;



				car.CarImages = await UploadImagesFromBase64Async(newCar.Images);


				await _db.cars.AddAsync(car);
				await _db.SaveChangesAsync();

				//return created car
				var cars = await _db.cars
						   .Include(c => c.Brand)
						   .Include(c => c.Model)
						   .Include(c => c.Transmission)
						   .Include(c => c.CarImages)
						   .FirstOrDefaultAsync(c => c.Id == car.Id);
				response.Data = _mapper.Map<GetCarDTO>(car);

			}
			catch (DbUpdateException ex)
			{
				var innerException = ex.InnerException;
				var innerExceptionMessage = innerException?.Message;

				response.Success = false;
				response.Message = innerExceptionMessage;
			}
			return response;
		}

		public async Task<ServiceResponse<List<GetBrandsDTO>>> GetBrandsAndModels()
		{
			var response = new ServiceResponse<List<GetBrandsDTO>>();
			try
			{
				var brands = await _db.Brands
							 .Include(c => c.models)
							.ToListAsync();
				response.Data = brands.Select(c => _mapper.Map<GetBrandsDTO>(c)).ToList();
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}
			return response;
		}

		public async Task<ServiceResponse<List<GetCarDTO>>> GetAllCars()
		{
			var response = new ServiceResponse<List<GetCarDTO>>();
			try
			{
				var cars = await _db.cars
								 .Include(c => c.Brand)
								 .Include(c => c.Model)
								 .Include(c => c.Transmission)
								 .Include(c => c.CarImages)
								 .Include(c => c.User)	
								 .OrderByDescending(c=>c.Id)
								 .ToListAsync();
				response.Data = cars.Select(c => _mapper.Map<GetCarDTO>(c)).ToList();
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}
			return response;
		}

		public async Task<ServiceResponse<List<GetTransmissionsDTO>>> GetTransmissions()
		{
			var response = new ServiceResponse<List<GetTransmissionsDTO>>();
			try
			{
				var transmissions = await _db.Transmissions.ToListAsync();
				response.Data = transmissions.Select(c => _mapper.Map<GetTransmissionsDTO>(c)).ToList();
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}
			return response;
		}

		public async Task<ServiceResponse<List<GetCarDTO>>> GetUserLikedCars(string email)
		{
			var response = new ServiceResponse<List<GetCarDTO>>();
			try
			{
				var user = await _db.users.FirstOrDefaultAsync(c => c.Email == email);

				var cars = await _db.cars
			   .Include(c => c.Brand)
			   .Include(c => c.Model)
			   .Include(c => c.Transmission)
			   .Include(c => c.CarImages)
			   .Include(c => c.User)
			   .Where(c => c.UserLikedCars.Any(uc => uc.UserId == user.Id))
			   .ToListAsync();



				response.Data = cars.Select(c => _mapper.Map<GetCarDTO>(c)).ToList();

			}
			catch (DbUpdateException ex)
			{
				var innerException = ex.InnerException;
				var innerExceptionMessage = innerException?.Message;

				response.Success = false;
				response.Message = innerExceptionMessage;
			}
			return response;
		}

		public async Task<ServiceResponse<string>> RentCar(RentCarDTO rent)
		{
			var response = new ServiceResponse<string>();
			try
			{
				var user = await _db.users.FirstOrDefaultAsync(c => c.Email == rent.UserEmail);
				var car = await _db.cars
					.Include(c => c.User)
					.FirstOrDefaultAsync(c => c.Id == rent.CarId);

				if (user == null || car == null)
				{
					response.Success = false;
					response.Message = "User or car not found.";
					return response;
				}
				if (car.IsRented)
				{
					response.Success = false;
					response.Message = "Car is already rented!";
					return response;
				}
				if (user.Amount < rent.Price)
				{
					response.Success = false;
					response.Message = "Insufficient funds";
					return response;
				}

				RentedCars rentedCar = _mapper.Map<RentedCars>(rent);
				rentedCar.RentEndDate = DateTime.UtcNow.AddDays(rent.RentDays);
				rentedCar.LastUpdated = DateTime.UtcNow;

				user.RentedCars.Add(rentedCar);


				car.IsRented = true;
				user.Amount -= rent.Price;
				car.User.Amount += rent.Price;
				await _db.SaveChangesAsync();
				response.Data = "Car was rented. Check your profile!";
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}
			return response;
		}
		private async Task<List<CarImages>> UploadImagesFromBase64Async(List<string> base64Strings)
		{
			Account account = CloudinaryConfig.GetAccount();
			Cloudinary cloudinary = new Cloudinary(account);

			var uploadTasks = base64Strings.Select(async base64String =>
			{
				byte[] imageBytes = Convert.FromBase64String(base64String);

				using (MemoryStream ms = new MemoryStream(imageBytes))
				{
					ImageUploadParams uploadParams = new ImageUploadParams()
					{
						File = new FileDescription("image", ms),
						UploadPreset = _configuration.GetSection("Cloudinary:UploadPreset").Value
					};

					ImageUploadResult uploadResult = await cloudinary.UploadAsync(uploadParams);

					return new CarImages
					{
						ImageUrl = uploadResult.Url.ToString()
					};
				}
			});

			return (await Task.WhenAll(uploadTasks)).ToList();
		}

		private void DeleteImageFromCloudinary(string imageUrl)
		{
			Account account = CloudinaryConfig.GetAccount();
			Cloudinary cloudinary = new Cloudinary(account);

			// Extract the public ID from the image URL
			string publicId = ExtractPublicIdFromImageUrl(imageUrl);

			// Delete the image using the complete public ID
			DeletionParams deletionParams = new DeletionParams(publicId)
			{
				Invalidate = true,
		 
			};

			DeletionResult deletionResult = cloudinary.Destroy(deletionParams);


		}

		private string ExtractPublicIdFromImageUrl(string imageUrl)
		{
			// Extract the public ID using regular expression
			string pattern = @"(?<=\/v\d+\/)(.*?)(?=\.)";
			Match match = Regex.Match(imageUrl, pattern);

			if (match.Success)
			{
				return match.Value;
			}
			else
			{
				// Invalid image URL
				throw new Exception("Invalid image URL: " + imageUrl);
			}
		}


		public async Task<ServiceResponse<List<GetCarDTO>>> GetPopularCars()
		{
			var response = new ServiceResponse<List<GetCarDTO>>();
			try
			{
				var cars = await _db.cars
								 .Include(c => c.Brand)
								 .Include(c => c.Model)
								 .Include(c => c.Transmission)
								 .Include(c => c.CarImages)
								 .Include(c => c.User)
								 .Where(c => c.Likes >= 10)
								 .OrderByDescending(c => c.Likes)
								 .ToListAsync();
				response.Data = cars.Select(c => _mapper.Map<GetCarDTO>(c)).ToList();
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}
			return response;
		}

		public async Task<ServiceResponse<string>> AddCarToFavorites(string email, int carId)
		{
			var response = new ServiceResponse<string>();
			try
			{
				var user = await _db.users.FirstOrDefaultAsync(u => u.Email == email);
				var car = await _db.cars.FirstOrDefaultAsync(c => c.Id == carId);


				if (user == null || car == null)
				{
					response.Success = false;
					response.Message = "User or car not found.";
					return response;
				}

				var userLikedCar = new UserLikedCar { Car = car };
				user.UserLikedCars.Add(userLikedCar);
				await _db.SaveChangesAsync();
				response.Data = "Car was added to your favorites!";

			}
			catch (DbUpdateException ex)
			{
				if (ex.InnerException is SqlException sqlException && sqlException.Number == 2627)
				{
					response.Success = false;
					response.Message = "This car is already added to your favorites.";
				}
				else
				{
					response.Success = false;
					response.Message = ex.Message;
				}


			}
			return response;
		}

		public async Task<ServiceResponse<string>> UnlikeCar(string email, int carId)
		{
			var response = new ServiceResponse<string>();

			try
			{
				var user = await _db.users
							  .Include(u => u.UserLikedCars)
							  .FirstOrDefaultAsync(u => u.Email == email);
				var deleteCar = await _db.UserLikedCar.FirstAsync(c => c.CarId == carId && c.UserId == user.Id);

				if (deleteCar != null)
				{

					_db.UserLikedCar.Remove(deleteCar);
					await _db.SaveChangesAsync();
					response.Data = "Car was removed from favorites!";
				}
				else
				{
					response.Success = false;
					response.Message = "The specified car was not found in favorites.";
				}
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public async Task<ServiceResponse<GetCarDTO>> GetCar(int carId)
		{
			var response = new ServiceResponse<GetCarDTO>();
			try
			{
				var car = await _db.cars
								.Include(c => c.Brand)
								.Include(c => c.Transmission)
								.Include(c => c.Model)
								.Include(c => c.CarImages)
								.Include(c => c.User)
								.FirstOrDefaultAsync(c => c.Id == carId);
				if (car == null)
				{
					response.Success = false;
					response.Message = "Car was not found!";
				}
				response.Data = _mapper.Map<GetCarDTO>(car);
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}
			return response;
		}
		public async Task<ServiceResponse<List<GetRentedCarDTO>>> GetRentedCars(string userEmail)
		{
			var response = new ServiceResponse<List<GetRentedCarDTO>>();
			try
			{
				var user = await _db.users.FirstOrDefaultAsync(c => c.Email == userEmail);

				var cars = await _db.cars
			   .Include(c => c.Brand)
			   .Include(c => c.Model)
			   .Include(c => c.Transmission)
			   .Include(c => c.CarImages)
			   .Include(c => c.RentedCar)
			   .Where(c => c.RentedCar.Any(uc => uc.User.Id == user.Id))
			   .OrderByDescending(c => c.Id)
			   .ToListAsync();



				response.Data = cars.Select(c => _mapper.Map<GetRentedCarDTO>(c)).ToList();
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}
			return response;

		}

		public async Task<ServiceResponse<List<GetCarDTO>>> FilterCars(FilterDTO car)
		{
			var response = new ServiceResponse<List<GetCarDTO>>();
			try
			{
				var Cars = _db.cars
				 .Include(c => c.Brand)
				 .Include(c => c.Model)
				 .Include(c => c.Transmission)
				 .Include(c => c.CarImages)
				 .AsQueryable();

				// Apply filters based on the provided values
				// The query parameters are used to protect against SQL injection attacks.
				if (!string.IsNullOrEmpty(car.City))
				{
					string cityParam = car.City.ToLower();
					Cars = Cars.Where(c => c.City.ToLower() == car.City.ToLower());
				}
				if (!string.IsNullOrEmpty(car.Brand))
				{
					string brandParam = car.Brand;
					Cars = Cars.Where(c => c.Brand.Name == car.Brand);
				}

				if (car.StartYear.HasValue)
				{
					int startYearParam = car.StartYear.Value;
					Cars = Cars.Where(c => c.Year >= car.StartYear.Value);
				}

				if (car.EndYear.HasValue)
				{
					int endYearParam = car.EndYear.Value;
					Cars = Cars.Where(c => c.Year <= car.EndYear.Value);
				}

				if (car.MinPrice.HasValue)
				{
					decimal minPriceParam = car.MinPrice.Value;
					Cars = Cars.Where(c => c.Price >= car.MinPrice.Value);
				}

				if (car.MaxPrice.HasValue)
				{
					decimal maxPriceParam = car.MaxPrice.Value;
					Cars = Cars.Where(c => c.Price <= car.MaxPrice.Value);
				}

				if (car.Capacity.HasValue)
				{
					int capacityParam = car.Capacity.Value;
					Cars = Cars.Where(c => c.Capacity == car.Capacity.Value);
				}

				var filteredCars = await Cars.Select(c => _mapper.Map<GetCarDTO>(c)).ToListAsync();
				response.Data = filteredCars;

			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}
			return response;
		}

		public async Task<ServiceResponse<List<GetCarDTO>>> GetUserAddedCars(string userEmail)
		{
			var response = new ServiceResponse<List<GetCarDTO>>();
			try
			{
				var user = await _db.users.FirstOrDefaultAsync(c => c.Email == userEmail);
				var cars = await _db.cars
								 .Include(c => c.Brand)
								 .Include(c => c.Model)
								 .Include(c => c.Transmission)
								 .Include(c => c.CarImages)
								 .Include(c => c.User)
								 .Where(c => c.User.Id == user.Id)
								 .OrderByDescending(c => c.Id)
								 .ToListAsync();
				response.Data = cars.Select(c => _mapper.Map<GetCarDTO>(c)).ToList();
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}
			return response;
		}

		public async Task<ServiceResponse<string>> RemoveCar(int carId)
		{
			var response = new ServiceResponse<string>();
			try
			{
				var car = await _db.cars
					.Include(c => c.CarImages)
					.FirstOrDefaultAsync(c => c.Id == carId);

				//delete car images from cloudinary server
				foreach (var image in car.CarImages)
					DeleteImageFromCloudinary(image.ImageUrl);

				//remove car from database
				_db.cars.Remove(car);
				await _db.SaveChangesAsync();
				response.Data = "Car was deleted!";
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}
	}
}
