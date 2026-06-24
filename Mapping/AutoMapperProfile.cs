
using Assignment2.Models;
using Assignment2.ViewModels;
using AutoMapper;
using UserManagement.Services.Models.Account;
using UserManagement.Services.Models.User;

namespace Assignment2.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            
            CreateMap<RegisterViewModel, ApplicationUser>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.Email));

            CreateMap<InviteUserViewModel, ApplicationUser>();

            CreateMap<ApplicationUser, InviteUserViewModel>();

            CreateMap<UserViewModel, ApplicationUser>();

            CreateMap<ApplicationUser, UserViewModel>();

            CreateMap<RegisterViewModel, Register>();

            CreateMap<LoginViewModel, Login>();

            CreateMap<InviteUserViewModel, InviteUser>();

            CreateMap<SetPasswordViewModel, SetPassword>();

            CreateMap<UserViewModel, UpdateUser>();


            CreateMap<Register, ApplicationUser>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.Email));

            CreateMap<InviteUser, ApplicationUser>();

            CreateMap<UpdateUser, ApplicationUser>();
            CreateMap<ApplicationUser, User>();


           

            CreateMap<User, UserViewModel>();

            CreateMap<UserList, UserListViewModel>();


            CreateMap<UserViewModel, User>();

            CreateMap<UserListViewModel, UserList>();
        }
    }
}