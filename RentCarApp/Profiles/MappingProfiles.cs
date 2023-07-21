using AutoMapper;
using RentCarApp.Dtos.Auth;
using RentCarApp.Dtos.Cars;
using RentCarApp.Dtos.Notifications;
using RentCarApp.Models.Car;
using RentCarApp.Models.user;
using RentCarApp.Models.User;

namespace RentCarApp.Profiles
{
	public class MappingProfiles : Profile
	{
				
		public MappingProfiles()
		{			
			CreateMap<UserRegisterDTO, User>().ReverseMap();
			CreateMap<GetUserDTO, User>().ReverseMap();		
			CreateMap<GetBrandsDTO, Brand>().ReverseMap();
			CreateMap<GetModelDTO, CarModel>().ReverseMap();
			CreateMap<GetTransmissionsDTO, Transmission>().ReverseMap();
			CreateMap<GetCarDTO, Cars>().ReverseMap();
			CreateMap<AddCarDTO, Cars>().ReverseMap();
			CreateMap<GetBrandDTO, Brand>().ReverseMap();
			CreateMap<GetImagesDTO, CarImages>().ReverseMap();
			CreateMap<RentedCars, RentCarDTO>().ReverseMap();
			CreateMap<RentedCars, GetRentedCarDTO>().ReverseMap();
			CreateMap<GetNotificationsDTO, Notification>().ReverseMap();
			CreateMap<CheckNewUserRentNotificationsDTO, Notification>().ReverseMap();
			CreateMap<Cars, GetRentedCarDTO>()
		.ForMember(dest => dest.RentedCar, opt => opt.MapFrom(src => src.RentedCar.FirstOrDefault()));
		}

	}
}
