namespace RentCarApp.Services.Interfaces
{
	public interface IEmailSender
	{
		Task SendEmailAsync(string email, string subject, string message);
		Task SendEmail(string email, string subject, string message);
	}
}
