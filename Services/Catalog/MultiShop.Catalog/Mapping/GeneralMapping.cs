using AutoMapper;
using MultiShop.Catalog.Dtos.CategoryDtos;
using MultiShop.Catalog.Dtos.ProductDetailDtos;
using MultiShop.Catalog.Dtos.ProductDtos;
using MultiShop.Catalog.Dtos.ProductImageDtos;
using MultiShop.Catalog.Entities;

namespace MultiShop.Catalog.Mapping;

public class GeneralMapping : Profile
{
    public GeneralMapping()
    {
        CreateMap<Category, ResultCategoryDto>();
        CreateMap<UpdateCategoryDto, Category>();
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<Category, GetByIdCategoryDto>();

        CreateMap<Product, ResultProductDto>();
        CreateMap<UpdateProductDto, Product>();
        CreateMap<CreateProductDto, Product>();
        CreateMap<Product, GetByIdProductDto>();
        CreateMap<Product, ResultProductWithCategory>()
            .ForMember(
                dest => dest.CategoryName,
                opt => opt.MapFrom(src =>
                    src.Category != null
                        ? src.Category.CategoryName
                        : "Kategori Yok"
                )
            );

        CreateMap<ProductDetail, ResultProductDetailDto>();
        CreateMap<UpdateProductDetailDto, ProductDetail>();
        CreateMap<CreateProductDetailDto, ProductDetail>();
        CreateMap<ProductDetail, GetByIdProductDetailDto>();

        CreateMap<ProductImage, ResultProductImageDto>();
        CreateMap<UpdateProductImageDto, ProductImage>();
        CreateMap<CreateProductImageDto, ProductImage>();
        CreateMap<ProductImage, GetByIdProductImageDto>();
    }
}