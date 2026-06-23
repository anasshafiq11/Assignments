
using Assignment2.Models;
using Assignment2.ViewModels;
using AutoMapper;
using UserManagement.DTOs.Account;
using UserManagement.DTOs.User;

namespace Assignment2.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // =========================
            // Entity <-> ViewModel
            // =========================

            CreateMap<RegisterViewModel, ApplicationUser>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.Email));

            CreateMap<InviteUserViewModel, ApplicationUser>();

            CreateMap<ApplicationUser, InviteUserViewModel>();

            CreateMap<UserViewModel, ApplicationUser>();

            CreateMap<ApplicationUser, UserViewModel>();


            // =========================
            // ViewModel -> DTO
            // =========================

            CreateMap<RegisterViewModel, RegisterDto>();

            CreateMap<LoginViewModel, LoginDto>();

            CreateMap<InviteUserViewModel, InviteUserDto>();

            CreateMap<SetPasswordViewModel, SetPasswordDto>();

            CreateMap<UserViewModel, UpdateUserDto>();


            // =========================
            // DTO -> Entity
            // =========================

            CreateMap<RegisterDto, ApplicationUser>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.Email));

            CreateMap<InviteUserDto, ApplicationUser>();

            CreateMap<UpdateUserDto, ApplicationUser>();


            // =========================
            // Entity -> DTO
            // =========================

            CreateMap<ApplicationUser, UserDto>();


           

            CreateMap<UserDto, UserViewModel>();

            CreateMap<UserListDto, UserListViewModel>();


            // =========================
            // Optional Reverse Maps
            // =========================

            CreateMap<UserViewModel, UserDto>();

            CreateMap<UserListViewModel, UserListDto>();
        }
    }
}