using AutoMapper;
using ProductsGuradution.Dtos;
using ProductsGuradution.Models;

namespace ProductsGuradution.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDetailsDto>();
            CreateMap<ProductDto, Product>()
                .ForMember(src => src.CatImg, opt => opt.Ignore());
        }
    }
}
