using AutoMapper;
using MultiShop.Catalog.Dtos.BrandDtos;
using MultiShop.Catalog.Dtos.CategoryDtos;
using MultiShop.Catalog.Dtos.FeaturedDtos;
using MultiShop.Catalog.Dtos.FeatureSldierDtos;
using MultiShop.Catalog.Dtos.OfferDiscountDtos;
using MultiShop.Catalog.Dtos.ProductDetailDtos;
using MultiShop.Catalog.Dtos.ProductDtos;
using MultiShop.Catalog.Dtos.ProductImageDtos;
using MultiShop.Catalog.Dtos.SpecialOfferDtos;
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
        
        CreateMap<FeatureSlider, ResultFeatureSliderDto>();
        CreateMap<UpdateFeatureSliderDto, FeatureSlider>().ReverseMap();
        CreateMap<CreateFeatureSliderDto, FeatureSlider>();
        
        CreateMap<SpecialOffer, ResultSpecialOfferDto>();
        CreateMap<UpdateSpecialOfferDto, SpecialOffer>().ReverseMap();
        CreateMap<CreateSpecialOfferDto, SpecialOffer>();

        CreateMap<Featured, ResultFeaturedDto>();
        CreateMap<UpdateFeaturedDto, Featured>().ReverseMap();
        CreateMap<CreateFeaturedDto, Featured>();
        
        CreateMap<OfferDiscount, ResultOfferDiscountDto>();
        CreateMap<UpdateOfferDiscountDto, OfferDiscount>().ReverseMap();
        CreateMap<CreateOfferDiscountDto, OfferDiscount>();
        
        CreateMap<Brand, ResultBrandDto>();
        CreateMap<UpdateBrandDto, Brand>().ReverseMap();
        CreateMap<CreateBrandDto, Brand>();
    }
}