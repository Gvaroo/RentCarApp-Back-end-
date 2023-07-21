using Microsoft.EntityFrameworkCore;
using RentCarApp.Data;
using RentCarApp.Models;
using RentCarApp.Services.Interfaces;

namespace RentCarApp.Services.Implementations
{
	public class BalanceService : IBalanceService
	{
		private readonly ApplicationDbContext _db;
		public BalanceService(ApplicationDbContext db)
		{
			_db = db;
		}
		public async Task<ServiceResponse<string>> AddBalance(string email, int amount)
		{
			var response = new ServiceResponse<string>();
			try
			{
				var user = await _db.users.FirstOrDefaultAsync(c => c.Email == email);
				if (user == null)
				{
					response.Success = false;
					response.Message = "User not found";
					return response;
				}
				user.Amount += amount;
				await _db.SaveChangesAsync();
				response.Data = "Balance is added!";
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
