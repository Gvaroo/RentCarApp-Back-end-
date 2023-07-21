using RentCarApp.Models;

namespace RentCarApp.Services.Interfaces
{
	public interface IBalanceService
	{
		Task<ServiceResponse<string>> AddBalance(string email, int amount);
	}
}
