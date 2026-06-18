
using Assignment2.DTOs.Product;
using Assignment2.Models;
using Assignment2.ViewModels;
using AutoMapper;

namespace Assignment2.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProductDto, Product>();
            CreateMap<Product, ProductDto>();
            CreateMap<RegisterViewModel, ApplicationUser>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.Email));

            CreateMap<InviteUserViewModel, ApplicationUser>();

            CreateMap<ApplicationUser, InviteUserViewModel>();
            CreateMap<UserViewModel, ApplicationUser>();
            CreateMap<ApplicationUser, UserViewModel>();
        }
    }
}