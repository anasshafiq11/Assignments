using AutoMapper;
using UsersApi.Common.Querying;
using UsersApi.DTOs;
using UsersApi.Models;

namespace UsersApi.Mapping
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateUserDto, User>();

            CreateMap<UpdateUserDto, User>();

            CreateMap<User, UserResponseDto>();
            
        }
    }
}
