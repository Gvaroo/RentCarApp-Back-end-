using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RentCarApp.Data;
using RentCarApp.Dtos.Notifications;
using RentCarApp.Models;
using RentCarApp.Models.user;
using RentCarApp.Services.Interfaces;

namespace RentCarApp.Services.Implementations
{
	public class NotificationService : INotificationService
	{
		private readonly ApplicationDbContext _db;
		private readonly IMapper _mapper;

		public NotificationService(ApplicationDbContext db, IMapper mapper)
		{
			_db = db;
			_mapper = mapper;
		}

		public async Task<ServiceResponse<List<CheckNewUserRentNotificationsDTO>>> CheckNewRentNotifications(string userEmail)
		{
			var response = new ServiceResponse<List<CheckNewUserRentNotificationsDTO>>();
			try
			{
				var user = await _db.users
								.Include(c => c.Notifications)
								.FirstOrDefaultAsync(c => c.Email == userEmail);
				if (user == null)
				{
					response.Success = false;
					response.Message = "User not found";
					return response;
				}

				var notifications = user.Notifications.OrderByDescending(c => c.Id).ToList();
				response.Data = notifications.Select(c => _mapper.Map<CheckNewUserRentNotificationsDTO>(c)).ToList();

			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}
			return response;
		}

		public async Task<ServiceResponse<List<GetNotificationsDTO>>> GetUserRentNotifications(string userEmail)
		{
			var response = new ServiceResponse<List<GetNotificationsDTO>>();
			try
			{
				var user = await _db.users
								.Include(c => c.Notifications)
								.FirstOrDefaultAsync(c => c.Email == userEmail);
				if (user == null)
				{
					response.Success = false;
					response.Message = "User not found";
					return response;
				}
				var notifications = user.Notifications.OrderByDescending(c => c.Id).ToList();
				//change notifications to IsRead
				notifications.ForEach(notif => notif.IsRead = true);
				await _db.SaveChangesAsync();
				response.Data = notifications.Select(c => _mapper.Map<GetNotificationsDTO>(c)).ToList();

			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;

			}
			return response;
		}

		public async Task<ServiceResponse<string>> SendRentNotification(SendRentNotificationDTO notification)
		{
			var response = new ServiceResponse<string>();
			try
			{
				var user = await _db.users.FirstOrDefaultAsync(c => c.Email == notification.UserEmail);
				var car = await _db.cars.FirstOrDefaultAsync(c => c.Id == notification.CarId);

				if (user == null || car == null)
				{
					response.Success = false;
					response.Message = "User or car not found.";
					return response;
				}
				var message = $"Your car {car.Name} was rented for {notification.RentDays} days!";
				var rentNotification = new Notification { Message = message };
				user.Notifications.Add(rentNotification);
				await _db.SaveChangesAsync();
				response.Data = "Notification was send successfully";
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
