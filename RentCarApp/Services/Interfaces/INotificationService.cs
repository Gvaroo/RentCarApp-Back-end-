using RentCarApp.Dtos.Notifications;
using RentCarApp.Models;

namespace RentCarApp.Services.Interfaces
{
	public interface INotificationService
	{
		Task<ServiceResponse<string>> SendRentNotification(SendRentNotificationDTO notification);
		Task<ServiceResponse<List<GetNotificationsDTO>>> GetUserRentNotifications(string userEmail);
		Task<ServiceResponse<List<CheckNewUserRentNotificationsDTO>>> CheckNewRentNotifications(string userEmail);
	}
}
