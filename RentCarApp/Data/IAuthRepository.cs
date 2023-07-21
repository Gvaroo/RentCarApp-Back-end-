using RentCarApp.Dtos.Auth;
using RentCarApp.Models;
using RentCarApp.Models.User;

namespace RentCarApp.Data
{
	public interface IAuthRepository
	{
		Task<ServiceResponse<int>> Register(UserRegisterDTO user);
		Task<ServiceResponse<UserInfoDTO>> Login(string email, string password);
		Task<ServiceResponse<GetUserDTO>> GetUser(string username);
		Task<bool> UserExists(string email);
		Task<bool> UserPhoneNumberExists(int phoneNumber);
		Task<ServiceResponse<string>> VerifyUserEmail(string token);
		Task<ServiceResponse<string>> VerifyUserCode(string code);
	}
}
