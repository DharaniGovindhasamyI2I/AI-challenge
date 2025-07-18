using AutoMapper;
using CleanArchitecture.Application.Products.Commands.CreateProduct;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Products.Common
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
} 