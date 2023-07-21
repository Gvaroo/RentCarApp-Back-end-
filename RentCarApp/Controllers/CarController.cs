using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentCarApp.Dtos.Cars;
using RentCarApp.Services.Interfaces;

namespace RentCarApp.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class CarController : ControllerBase
	{
		private readonly ICarService _carService;

		public CarController(ICarService carService)
		{
			_carService = carService;
		}

		[HttpGet]
		public async Task<IActionResult> GetBrandsAndModels()
		{
			var result = await _carService.GetBrandsAndModels();

			return result.Success ? Ok(result) : BadRequest(result);
		}
		[HttpGet]
		public async Task<IActionResult> GetTransmissions()
		{
			var result = await _carService.GetTransmissions();

			return result.Success ? Ok(result) : BadRequest(result);
		}
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> AddCar(AddCarDTO newCar)
		{
			var result = await _carService.AddCar(newCar);
			return result.Success ? Ok(result) : BadRequest(result);
		}
		[HttpGet]
		public async Task<IActionResult> GetAllCars()
		{
			var result = await _carService.GetAllCars();
			return result.Success ? Ok(result) : BadRequest(result);
		}
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetUserLikedCars(string email)
		{
			var result = await _carService.GetUserLikedCars(email);
			return result.Success ? Ok(result) : BadRequest(result);
		}
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> RentCar(RentCarDTO rent)
		{
			var result = await _carService.RentCar(rent);
			return result.Success ? Ok(result) : BadRequest(result);
		}
		[HttpGet]
		public async Task<IActionResult> GetPopularCars()
		{
			var result = await _carService.GetPopularCars();
			return result.Success ? Ok(result) : BadRequest(result);
		}
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> AddCarToFavorites(string email, int carId)
		{
			var result = await _carService.AddCarToFavorites(email, carId);
			return result.Success ? Ok(result) : BadRequest(result);
		}
		[HttpDelete]
		[Authorize]
		public async Task<IActionResult> UnlikeCar(string email, int carId)
		{
			var result = await _carService.UnlikeCar(email, carId);
			return result.Success ? Ok(result) : BadRequest(result);
		}
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetCar(int carId)
		{
			var result = await _carService.GetCar(carId);
			return result.Success ? Ok(result) : BadRequest(result);
		}
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetUserRentedCars(string email)
		{
			var result = await _carService.GetRentedCars(email);
			return result.Success ? Ok(result) : BadRequest(result);
		}
		[HttpPost]
		public async Task<IActionResult> FilterCars(FilterDTO car)
		{
			var result = await _carService.FilterCars(car);
			return result.Success ? Ok(result) : BadRequest(result);
		}
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetUserAddedCars(string email)
		{
			var result = await _carService.GetUserAddedCars(email);
			return result.Success ? Ok(result) : BadRequest(result);
		}
		[HttpDelete]
		[Authorize]
		public async Task<IActionResult> RemoveCar(int carId)
		{
			var result = await _carService.RemoveCar(carId);
			return result.Success ? Ok(result) : BadRequest(result);
		}
	}
}
