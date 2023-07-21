using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentCarApp.Services.Interfaces;

namespace RentCarApp.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class BalanceController : ControllerBase
	{
		private readonly IBalanceService _balanceService;
		public BalanceController(IBalanceService balanceService)
		{
			_balanceService = balanceService;
		}
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> AddBalance(string email, int amount)
		{
			var result = await _balanceService.AddBalance(email, amount);
			return result.Success ? Ok(result) : BadRequest(result);
		}
	}
}
