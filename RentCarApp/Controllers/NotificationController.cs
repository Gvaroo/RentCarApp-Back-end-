using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentCarApp.Dtos.Notifications;
using RentCarApp.Services.Interfaces;

namespace RentCarApp.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class NotificationController : ControllerBase
	{
		private readonly INotificationService _notificationService;

		public NotificationController(INotificationService notificationService)
		{
			_notificationService = notificationService;
		}
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> SendRentNotification(SendRentNotificationDTO notification)
		{
			var result = await _notificationService.SendRentNotification(notification);
			return result.Success ? Ok(result) : BadRequest(result);
		}
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetUserRentNotifications(string userEmail)
		{
			var result = await _notificationService.GetUserRentNotifications(userEmail);
			return result.Success ? Ok(result) : BadRequest(result);
		}
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> CheckNewRentNotifications(string userEmail)
		{
			var result = await _notificationService.CheckNewRentNotifications(userEmail);
			return result.Success ? Ok(result) : BadRequest(result);
		}
	}
}
