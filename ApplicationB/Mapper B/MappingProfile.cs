 using Microsoft.AspNetCore.Hosting;
using AutoMapper;
using DTOsB.Category;
using DTOsB.Product;
using DTOsB.Shared;
using Microsoft.AspNetCore.Http;
using ModelsB.Category_B;
using ModelsB.Product_B;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ModelsB.Localization_B;
using DTOsB.Order.OrderDTO;
using DTOsB.Order.OrderItemDTO;
using DTOsB.Order.PaymentDTO;
using DTOsB.Order.ShippingDTO;
using ModelsB.Order_B;
using DTOsB.Account;
using DTOsB.User;
using ModelsB.Authentication_and_Authorization_B;

namespace ApplicationB.Mapper_B
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {


            // Product --------------------------------


            // ProductB to ProductDto mapping
            CreateMap<ProductB, ProductDto>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ForMember(dest => dest.Specifications, opt => opt.MapFrom(src => src.Specifications))
                .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => src.Translations))
                .ReverseMap();
            CreateMap<ProductB, ProductCreateOrUpdateDto>()
                .ForMember(dest => dest.ImageFiles, opt => opt.Ignore()).ReverseMap();


            CreateMap<ProductImageB, ProductImageDto>().ReverseMap();
            CreateMap<ProductImageB, ProductImageCreateOrUpdateDto>()
            .ForMember(dest => dest.ImageFile, opt => opt.Ignore()).ReverseMap();

            
            // Mapping for product translations
            CreateMap<ProductTranslationB, ProductTranslationDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.LanguageId, opt => opt.MapFrom(src => src.LanguageId))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.BrandName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ReverseMap();


            //CreateMap<ProductSpecificationsB, ProductSpecificationDto>().ReverseMap();
            //CreateMap<ProductSpecificationTranslationB, ProductSpecificationTranslationDto>().ReverseMap();
            CreateMap<ProductSpecificationsB, ProductSpecificationDto>()
               .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => src.Translations)).ReverseMap();

            CreateMap<ProductSpecificationTranslationB, ProductSpecificationTranslationDto>()
                    .ForMember(dest => dest.TranslatedKey, opt => opt.MapFrom(src => src.TranslatedKey))
                    .ForMember(dest => dest.TranslatedValue, opt => opt.MapFrom(src => src.TranslatedValue))
                    .ForMember(dest => dest.LanguageId, opt => opt.MapFrom(src => src.LanguageId))
                    .ReverseMap();
            

            CreateMap<SpecificationStore, SpecificationStoreDto>().ReverseMap();
            CreateMap<ReviewB, ReviewDto>().ReverseMap();


            // Category --------------------------------


            //CreateMap<CategoryB, GetAllCategoriesDTO>().ReverseMap();
            //CreateMap<CategoryB, CreateOrUpdateCategoriesDTO>().ReverseMap();
            //CreateMap<CategoryTranslationB, CategoryTranslationDTO>().ReverseMap();
            //CreateMap<CategoryTranslationB, CreateCategoryTranslationDto>().ReverseMap();

            //CreateMap<CategoryB, GetAllCategoriesDTO>().ReverseMap();
            CreateMap<CategoryB, GetAllCategoriesDTO>()
                    .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => src.Translations))
                    .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl)).ReverseMap();


            CreateMap<CategoryB, CreateOrUpdateCategoriesDTO>()
                                                      .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                                                     .ReverseMap();
            CreateMap<CategoryTranslationB, CategoryTranslationDTO>().ReverseMap();
            CreateMap<CategoryTranslationB, CreateCategoryTranslationDto>().ReverseMap();

            //CreateMap<ProductCategoryB, ProductCategoryDto>().ReverseMap();
            CreateMap<ProductCategoryB, ProductCategoryDto>()
           .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
           .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product)).ReverseMap();


            // Map ResultView<CategoryB> to ResultView<CreateOrUpdateCategoriesDTO>
            CreateMap<ResultView<CategoryB>, ResultView<CreateOrUpdateCategoriesDTO>>()
                .ForMember(dest => dest.Entity, opt => opt.MapFrom(src => src.Entity));
            CreateMap<ResultView<CategoryB>, ResultView<GetAllCategoriesDTO>>()
                .ForMember(dest => dest.Entity, opt => opt.MapFrom(src => src.Entity));
            CreateMap<CategoryB, GetAllCategoriesDTO>()
                .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => src.Translations))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));
            CreateMap<ResultView<CategoryB>, ResultView<GetAllCategoriesDTO>>()
                .ForMember(dest => dest.Entity, opt => opt.MapFrom(src => src.Entity))
                .ForMember(dest => dest.IsSuccess, opt => opt.MapFrom(src => src.IsSuccess))
                .ForMember(dest => dest.Msg, opt => opt.MapFrom(src => src.Msg));

            // Order --------------------------------

            CreateMap<SelectOrderBDTO, AddOrUpdateOrderBDTO>().ReverseMap();
            CreateMap<OrderB, AddOrUpdateOrderBDTO>().ReverseMap();
            CreateMap<OrderB, SelectOrderBDTO>().ReverseMap();

            CreateMap<SelectOrderItemBDTO, AddOrUpdateOrderItemBDTO>().ReverseMap();
            CreateMap<OrderItemB, AddOrUpdateOrderItemBDTO>().ReverseMap();
            CreateMap<OrderItemB, SelectOrderItemBDTO>()
                .ForMember(dest=> dest.ProductName, opt=>opt.MapFrom(src=> src.Product.Translations.FirstOrDefault().Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.Product.StockQuantity))
                .ReverseMap();

            CreateMap<PaymentB, AddOrUpdatePaymentBDTO>().ReverseMap();
            CreateMap<PaymentB, SelectPaymentBDTO>().ReverseMap();

            CreateMap<ShippingB, AddOrUpdateShippingBDTO>().ReverseMap();
            CreateMap<ShippingB, SelectShippingBDTO>().ReverseMap();

            // User --------------------------------

            CreateMap<RegisterDto, ApplicationUserB>().ReverseMap();
            CreateMap<LoginDTO, ApplicationUserB>().ReverseMap();
            CreateMap<UserDto, ApplicationUserB>().ReverseMap();
            CreateMap<CheckNumDTo, ApplicationUserB>().ReverseMap();


            // Others --------------------------------

            CreateMap<LanguageB, LanguageDto>().ReverseMap();

        }



    }
}
