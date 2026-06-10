using AutoMapper;
using InlämningWebb1.Application.Features.Products.DTOs;
using InlämningWebb1.Domain.Entities;

namespace InlämningWebb1.Application.Common.Mappings;

/// <summary>
/// AutoMapper profile for the Product feature.
/// All mapping rules for Product live here — one place to maintain.
/// AutoMapper discovers this profile automatically via AddAutoMapper(Assembly).
/// </summary>
public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        // Product → ProductDto
        // Property names match exactly on both types, so AutoMapper maps them without extra configuration.
        // This one line handles both single-object and collection mappings:
        //   _mapper.Map<ProductDto>(product)
        //   _mapper.Map<IEnumerable<ProductDto>>(products)
        CreateMap<Product, ProductDto>();
    }
}
