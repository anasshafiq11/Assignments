
using AutoMapper;
using Assignment2.DTOs;
using Assignment2.Models;
using Assignment2.DTOs.Product;

namespace Assignment2.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProductDto, Product>();
            CreateMap<Product, ProductDto>();
        }
    }
}