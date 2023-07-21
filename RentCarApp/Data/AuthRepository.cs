using AutoMapper;
using Azure.Core;
using Hangfire.Storage.Monitoring;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RentCarApp.Dtos.Auth;
using RentCarApp.Models;
using RentCarApp.Models.User;
using RentCarApp.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Sockets;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;
using RentCarApp.Models.user;

namespace RentCarApp.Data
{
	public class AuthRepository : IAuthRepository
	{
		private readonly ApplicationDbContext _db;
		private readonly IConfiguration _configuration;
		private readonly IMapper _mapper;
		private readonly IEmailSender _emailSender;
		private readonly HttpContext _httpContext;


		public AuthRepository(ApplicationDbContext db, IConfiguration configuration, IMapper mapper, IEmailSender emailSender, IHttpContextAccessor httpContextAccessor)
		{
			_db = db;
			_configuration = configuration;
			_mapper = mapper;
			_emailSender = emailSender;
			_httpContext = httpContextAccessor.HttpContext;
		}

		public async Task<ServiceResponse<UserInfoDTO>> Login(string email, string password)
		{
			var response = new ServiceResponse<UserInfoDTO>();
			try
			{
				var user = await _db.users.FirstOrDefaultAsync(c => c.Email == email);
				if (user == null)
				{
					response.Success = false;
					response.Message = "User not found!";

				}
				else if (!VerifyPasswordHash(password, user.PasswordSalt, user.PasswordHash))
				{
					response.Success = false;
					response.Message = "Password incorrect!";
				}
				else
				{
					// Get IP address from HttpContext
					var remoteIpAddress = _httpContext.Connection.RemoteIpAddress;

					// Check if the IP address is IPv6 loopback
					var isIPv6Loopback = IPAddress.IsLoopback(remoteIpAddress) && remoteIpAddress.AddressFamily == AddressFamily.InterNetworkV6;

					// Get the IP address string without the scope ID
					var ipAddress = isIPv6Loopback ? remoteIpAddress.MapToIPv6().ToString() : remoteIpAddress.ToString();


					// Check User Ip address
					if (!await CheckUserIpAddress(user, ipAddress))
					{
						var userInfo = new UserInfoDTO()
						{
							Email = user.Email,
							Token = GenerateToken(user),
							EmailVerified = user.IsVerified,
							IpVerified = false
						};
						response.Data = userInfo;
					}
					else
					{
						var userInfo = new UserInfoDTO()
						{
							Email = user.Email,
							Token = GenerateToken(user),
							EmailVerified = user.IsVerified,
							IpVerified = true
						};
						response.Data = userInfo;
					}
				}

			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}
			return response;
		}

		//Checks if user logged in with same ip address. If not sends VerificationCode to email
		private async Task<bool> CheckUserIpAddress(User user, string ip)
		{
			if (user.LastLoginIpAddress == null)
			{
				user.LastLoginIpAddress = ip;
				await _db.SaveChangesAsync();
				return true;
			}
			else if (user.LastLoginIpAddress != ip)
			{
				user.IsVerified = false;
				// Generate the verification Code
				var verifyCode = GenerateRandomString(6);

				// Send the confirmation email
				await _emailSender.SendEmailAsync(user.Email, "Verify code Confirmation By Eentox", $"We have detected a login to your account from an unfamiliar location. If this was not you, please change your password immediately to secure your account. If it was you, please use this code to verify. please note that code has expiration date of 15 mins.<br/> code: {verifyCode}");
				var userVerify = new SecurityVerificationCodes { VerificationCode = verifyCode, ExpireDate = DateTime.UtcNow.AddMinutes(15) };
				user.SecurityVerificationCodes.Add(userVerify);
				await _db.SaveChangesAsync();
				return false;
			}
			return true;
		}

		public async Task<ServiceResponse<int>> Register(UserRegisterDTO User)
		{
			var response = new ServiceResponse<int>();
			var user = _mapper.Map<User>(User);
			try
			{
				if (await UserExists(user.Email))
				{
					response.Success = false;
					response.Message = "User already exists";
					return response;
				}
				if (await UserPhoneNumberExists(user.Number))
				{
					response.Success = false;
					response.Message = "Phone number is  already registered!";
					return response;
				}

				CreatePasswordHash(User.Password, out string passwordHash, out string passwordSalt);

				user.PasswordHash = passwordHash;
				user.PasswordSalt = passwordSalt;
				await _db.users.AddAsync(user);
				await _db.SaveChangesAsync();

				// Generate the Email Confirmation Token
				var confirmationToken = GenerateToken(user);

				//Get Web App Url
				var WebAppUrl = _configuration.GetSection("WebAppUrl:Development").Value;

				// Create the callback URL
				var callbackUrl = $"{WebAppUrl}/emailConfirmed?code={Uri.EscapeDataString(confirmationToken)}";

				// Send the confirmation email
				await _emailSender.SendEmailAsync(User.Email, "Email Confirmation By Eentox", $"Please confirm your email by clicking the link below:<br/><a href='{callbackUrl}'>{callbackUrl}</a>");

				response.Data = user.Id;


			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}
			return response;
		}



		public async Task<bool> UserExists(string email)
		{
			var user = await _db.users.FirstOrDefaultAsync(c => c.Email == email);
			if (user == null) return false;
			return true;

		}
		public async Task<bool> UserPhoneNumberExists(int phoneNumber)
		{
			var user = await _db.users.FirstOrDefaultAsync(c => c.Number == phoneNumber);
			if (user == null) return false;
			return true;

		}
		public async Task<ServiceResponse<GetUserDTO>> GetUser(string email)
		{
			var response = new ServiceResponse<GetUserDTO>();
			try
			{
				var user = await _db.users.FirstOrDefaultAsync(c => c.Email == email);
				response.Data = _mapper.Map<GetUserDTO>(user);
				response.Data.Amount = user.Amount;
				response.Data.IsVerified = user.IsVerified;
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}
			return response;
		}
		private void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
		{
			passwordSalt = BCrypt.Net.BCrypt.GenerateSalt();
			passwordHash = BCrypt.Net.BCrypt.HashPassword(password, passwordSalt);

		}
		private bool VerifyPasswordHash(string password, string passwordSalt, string passwordHash)
		{
			string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, passwordSalt);
			return hashedPassword == passwordHash;
		}
		private string GenerateToken(User user)
		{
			var claims = new List<Claim>()
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Email,user.Email.ToString())
			};
			var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
			var descriptor = new SecurityTokenDescriptor()
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddDays(1),
				SigningCredentials = credentials
			};
			var handler = new JwtSecurityTokenHandler();
			var token = handler.CreateToken(descriptor);
			return handler.WriteToken(token);
		}
		public async Task<ServiceResponse<string>> VerifyUserEmail(string token)
		{
			var response = new ServiceResponse<string>();
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value);
			var validationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ValidateIssuer = false,
				ValidateAudience = false
			};

			try
			{
				//Get user claims from token
				var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
				var claims = claimsPrincipal.Identity as ClaimsIdentity;

				var userId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;

				// Get user to verify their account
				var user = await _db.users.FirstOrDefaultAsync(c => c.Id == Convert.ToInt32(userId));

				// if user doesn't exit
				if (user == null)
				{
					response.Success = false;
					response.Message = "User doesnt exist anymore!";
					return response;
				}

				//Verify user account
				user.IsVerified = true;

				await _db.SaveChangesAsync();
				response.Data = $"Your email address <{user.Email}> has been verified";

			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}
		public async Task<ServiceResponse<string>> VerifyUserCode(string code)
		{
			var response = new ServiceResponse<string>();
			try
			{
				var user = await _db.users
					.Include(c => c.SecurityVerificationCodes)
					.FirstOrDefaultAsync(c => c.SecurityVerificationCodes.Any(b => b.VerificationCode == code && b.ExpireDate > DateTime.UtcNow));
				if (user == null)
				{
					response.Success = false;
					response.Message = "Code is expired or incorrect";
					return response;
				}

				//Verify user account
				user.IsVerified = true;

				//Delete verification code
				user.SecurityVerificationCodes.Clear();

				// Get IP address from HttpContext
				var remoteIpAddress = _httpContext.Connection.RemoteIpAddress;

				// Check if the IP address is IPv6 loopback
				var isIPv6Loopback = IPAddress.IsLoopback(remoteIpAddress) && remoteIpAddress.AddressFamily == AddressFamily.InterNetworkV6;

				// Get the IP address string without the scope ID
				var ipAddress = isIPv6Loopback ? remoteIpAddress.MapToIPv6().ToString() : remoteIpAddress.ToString();
				user.LastLoginIpAddress = ipAddress;

				await _db.SaveChangesAsync();
				response.Data = $"Verification was successful! Please log in";
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}
			return response;
		}
		private string GenerateRandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			var random = new Random();

			var result = new string(Enumerable.Repeat(chars, length)
				.Select(s => s[random.Next(s.Length)]).ToArray());

			return result;
		}

	}
}
