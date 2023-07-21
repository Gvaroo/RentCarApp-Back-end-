using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentCarApp.Data;
using RentCarApp.Dtos.Auth;
using RentCarApp.Models.User;

namespace RentCarApp.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthRepository _auth;

		public AuthController(IAuthRepository auth)
		{
			_auth = auth;
		}
		[HttpPost]
		public async Task<IActionResult> Register(UserRegisterDTO user)
		{
			var result = await _auth.Register(user);
			return result.Success ? Ok(result) : BadRequest(result);
		}
		[HttpPost]
		public async Task<IActionResult> Login(UserLoginDTO user)
		{
			var result = await _auth.Login(user.Email, user.Password);
			return result.Success ? Ok(result) : BadRequest(result);


		}
		[HttpGet("{email}")]
		[Authorize]
		public async Task<IActionResult> GetUser(string email)
		{
			var result = await _auth.GetUser(email);
			return result.Success ? Ok(result) : BadRequest(result);
		}
		[HttpPost]
		public async Task<IActionResult> VerifyUserEmail(string token)
		{
			var result = await _auth.VerifyUserEmail(token);
			return result.Success ? Ok(result) : BadRequest(result);
		}
		[HttpPost]
		public async Task<IActionResult> VerifyUserCode(string code)
		{
			var result = await _auth.VerifyUserCode(code);
			return result.Success ? Ok(result) : BadRequest(result);
		}
	}
}
