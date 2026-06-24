using AutoMapper;
using UsersApi.Common.Querying;
using UsersApi.DTOs;
using UsersApi.DTOs.Auth;
using UsersApi.Models;
using UsersApi.Services.Models;

namespace UsersApi.Mapping
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateUserDto, User>();

            CreateMap<UpdateUserDto, User>();

            CreateMap<User, UserResponseDto>();
            CreateMap<LoginDto, LoginRequest>();
        }
    }
}
