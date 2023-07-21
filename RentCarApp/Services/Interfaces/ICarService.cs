using RentCarApp.Dtos.Cars;
using RentCarApp.Models;

namespace RentCarApp.Services.Interfaces
{
	public interface ICarService
	{
		Task<ServiceResponse<List<GetBrandsDTO>>> GetBrandsAndModels();
		Task<ServiceResponse<List<GetTransmissionsDTO>>> GetTransmissions();
		Task<ServiceResponse<GetCarDTO>> AddCar(AddCarDTO car);
		Task<ServiceResponse<string>> RemoveCar(int carId);
		Task<ServiceResponse<List<GetCarDTO>>> GetAllCars();
		Task<ServiceResponse<GetCarDTO>> GetCar(int carId);
		Task<ServiceResponse<List<GetCarDTO>>> GetPopularCars();
		Task<ServiceResponse<List<GetCarDTO>>> GetUserLikedCars(string email);
		Task<ServiceResponse<string>> AddCarToFavorites(string email, int carId);
		Task<ServiceResponse<string>> RentCar(RentCarDTO rent);
		Task<ServiceResponse<string>> UnlikeCar(string email, int carId);
		Task<ServiceResponse<List<GetRentedCarDTO>>> GetRentedCars(string userEmail);
		Task<ServiceResponse<List<GetCarDTO>>> GetUserAddedCars(string userEmail);
		Task<ServiceResponse<List<GetCarDTO>>> FilterCars(FilterDTO car);
	}
}
