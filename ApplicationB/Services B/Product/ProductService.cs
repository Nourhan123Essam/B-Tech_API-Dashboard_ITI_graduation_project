using ApplicationB.Contracts_B;
using ApplicationB.Services_B.General;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DTOsB.Product;
using DTOsB.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ModelsB.Product_B;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ApplicationB.Services_B.User;

namespace ApplicationB.Services_B.Product
{
public class ProductService: IProductService
    {
       

        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILanguageService _languageService;
        
        private readonly IUserService _userService;
       
        

     public ProductService(IProductRepository productRepository, IMapper mapper, IUserService userService,
            ILanguageService languageService)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _userService = userService;
            _languageService = languageService;
          
        }

        public async Task<ResultView<ProductCreateOrUpdateDto>> CreateProductAsync(ProductCreateOrUpdateDto productDto)
        {
            if (productDto.Id > 0)
                return ResultView<ProductCreateOrUpdateDto>.Failure("Product already exists. Use update to modify it.");

            if (productDto.Price < 0)
                return ResultView<ProductCreateOrUpdateDto>.Failure("Product price must be a positive value.");

            if (productDto.StockQuantity < 0)
                return ResultView<ProductCreateOrUpdateDto>.Failure("Product Quantity must be a positive value.");


            var product = _mapper.Map<ProductB>(productDto);
            var createdProduct = await _productRepository.AddAsync(product);


         
            return ResultView<ProductCreateOrUpdateDto>.Success(_mapper.Map<ProductCreateOrUpdateDto>(createdProduct));
        }

        public async Task<ResultView<ProductCreateOrUpdateDto>> UpdateProductAsync(ProductCreateOrUpdateDto productDto)
        {
            if (productDto == null)
                return ResultView<ProductCreateOrUpdateDto>.Failure("Product must have data to be added");

            var existingProduct = await _productRepository.GetByIdAsync(productDto.Id);
            if (existingProduct == null || existingProduct.IsDeleted)
                return ResultView<ProductCreateOrUpdateDto>.Failure("Product not found. Unable to update.");

            if (productDto.Price < 0)
                return ResultView<ProductCreateOrUpdateDto>.Failure("Product price must be a positive value.");

            if (productDto.StockQuantity < 0)
                return ResultView<ProductCreateOrUpdateDto>.Failure("Product Quantity must be a positive value.");

            _mapper.Map(productDto, existingProduct);
            
            //existingProduct.UpdatedBy = _userService.GetCurrentUserId();

            await _productRepository.UpdateAsync(existingProduct);
            return ResultView<ProductCreateOrUpdateDto>.Success(productDto);
        }

        public async Task<ResultView<ProductDto>> DeleteProductAsync(int id)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
                return ResultView<ProductDto>.Failure("Product not found. Unable to delete.");

            existingProduct.IsDeleted = true;
            //_userService.GetCurrentUserId();
            //existingProduct.Updated = DateTime.Now;

            await _productRepository.UpdateAsync(existingProduct);
            return ResultView<ProductDto>.Success(null);
        }

        public async Task<ResultView<ProductCreateOrUpdateDto>> GetProductByIdAsync(int id)
        {
         
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return ResultView<ProductCreateOrUpdateDto>.Failure("Product not found.");

            var productDto = _mapper.Map<ProductCreateOrUpdateDto>(product);
            return ResultView<ProductCreateOrUpdateDto>.Success(productDto);
        }

        public async Task<ResultView<IEnumerable<ProductDto>>> GetAllProductsAsync()
        {
            var languageId = _languageService.GetCurrentLanguageCode();

            //var products = await _productRepository.GetAllAsync().Result.ToListAsync();

            var filteredProducts = await _productRepository.GetFilteredProductsAsync(languageId);

            //var filteredProducts =  products.Where(p => p.Translations.Any(t => t.Language.Id == languageId));
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(filteredProducts);
            return ResultView<IEnumerable<ProductDto>>.Success(productDtos);

        }


        public async Task<ResultView<IEnumerable<ProductDto>>> SearchProductsByNameAsync(string name)
        {
            if (name == null)
                return ResultView<IEnumerable<ProductDto>>.Failure("Try Again");
            var products = await _productRepository.SearchByNameAsync(name); 
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products); 

            return ResultView<IEnumerable<ProductDto>>.Success(productDtos); 
        }


        public async Task<EntityPaginatedB<ProductDto>> GetAllPaginatedAsync(int pageNumber, int Count)
        {
            var languageId = _languageService.GetCurrentLanguageCode(); // Get current language
            var products = await _productRepository.GetAllAsync();
            var productsQuery = products.Where(p => p.Translations.Any(t => t.LanguageId == languageId)).AsQueryable();
            //var productsQuery = await _productRepository.GetAllAsync(); 

            var totalCount = productsQuery.Count(); 

            var Resultproducts = productsQuery
                .Skip(Count * (pageNumber - 1))
                .Take(Count)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CreatedBy = p.CreatedBy,
                    Created = p.Created,
                    UpdatedBy = p.UpdatedBy,
                    Updated = p.Updated,
                    IsDeleted = p.IsDeleted,
                    Images = p.Images.Select(img => new ProductImageDto
                    {
                        Id = img.Id, 
                        Url = img.Url, 
                                       
                    }).ToList(),
                    Translations = p.Translations
                    .Select(t => new ProductTranslationDto
                    {
                        Id = t.Id, 
                        Name = t.Name, 
                        BrandName = t.BrandName, 
                        Description =t.Description                       
                    }).ToList(),
                    Specifications = p.Specifications.Select(s => new ProductSpecificationDto
                    {
                        Id = s.Id,
                        Translations = s.Translations
                        .Select( t=> new ProductSpecificationTranslationDto { 
                        Id = t.Id,
                        TranslatedKey = t.TranslatedKey, 
                        TranslatedValue = t.TranslatedValue 
                        })  .ToList(),               
                    }).ToList(),
                }).ToList(); 

            return new EntityPaginatedB<ProductDto>
            {
                Data = Resultproducts,
                CountAllItems = totalCount
            };
          
        }



        /// <summary>
        /// Services With View Model
        /// </summary>
        /// <param name="productViewModel"></param>
        /// <returns></returns>
        //public async Task<ResultView<ProductViewModel>> CreateProductModelAsync(ProductViewModel productViewModel)
        //{
        //    if (productViewModel.ProductId > 0)
        //        return ResultView<ProductViewModel>.Failure("Product already exists. Use update to modify it.");

        //    if (productViewModel.Price < 0)
        //        return ResultView<ProductViewModel>.Failure("Product price must be a positive value.");

        //    if (productViewModel.StockQuantity < 0)
        //        return ResultView<ProductViewModel>.Failure("Product Quantity must be a positive value.");

        //    var productEntity = _mapper.Map<ProductB>(productViewModel);

        //    productViewModel.CreatedBy = _userService.GetCurrentUserId();
           
        //    await _productRepository.AddAsync(productEntity);
        //    //await _unitOfWork.CompleteAsync(); // Save changes

        //    var result = _mapper.Map<ProductViewModel>(productEntity);
        //    return ResultView<ProductViewModel>.Success(result);
        //}

        //public async Task<ResultView<ProductViewModel>> GetProductModelByIdAsync(int productId)
        //{
        //    var productEntity = await _productRepository.GetByIdAsync(productId);

        //    if (productEntity == null)
        //    {
        //        return ResultView<ProductViewModel>.Failure("Product not found");
        //    }

           
        //    var productViewModel = _mapper.Map<ProductViewModel>(productEntity);
        //    return ResultView<ProductViewModel>.Success(productViewModel);
        //}

        //public async Task<ResultView<ProductViewModel>> UpdateProductModelAsync(ProductViewModel productViewModel)
        //{
        //    if (productViewModel == null)
        //        return ResultView<ProductViewModel>.Failure("Product must have data to be added");

        //    if (productViewModel.Price < 0)
        //        return ResultView<ProductViewModel>.Failure("Product price must be a positive value.");

        //    if (productViewModel.StockQuantity < 0)
        //        return ResultView<ProductViewModel>.Failure("Product Quantity must be a positive value.");

        //    var productEntity = await _productRepository.GetByIdAsync(productViewModel.ProductId);

        //    if (productEntity == null || productEntity.IsDeleted)
        //    {
        //        return ResultView<ProductViewModel>.Failure("Product not found");
        //    }

        //    _mapper.Map(productViewModel, productEntity);

        //    productEntity.UpdatedBy = _userService.GetCurrentUserId();

            
        //    _productRepository.UpdateAsync(productEntity);
        //    //await _unitOfWork.CompleteAsync();

        //    var updatedProductViewModel = _mapper.Map<ProductViewModel>(productEntity);
        //    return ResultView<ProductViewModel>.Success(updatedProductViewModel);
        //}

        public async Task<ResultView<bool>> DeleteProductModelAsync(int productId)
        {
            //var existingProduct = await _productRepository.GetByIdAsync(id);
            //if (existingProduct == null)
            //    return ResultView<ProductDto>.Failure("Product not found. Unable to delete.");

            //existingProduct.IsDeleted = true;
            //_userService.GetCurrentUserId();
            //existingProduct.Updated = DateTime.Now;

            //await _productRepository.UpdateAsync(existingProduct);
            //return ResultView<ProductDto>.Success(null);


            var productEntity = await _productRepository.GetByIdAsync(productId);

            if (productEntity == null)
            {
                return ResultView<bool>.Failure("Product not found. Unable to delete.");
            }

            // Remove the product
            productEntity.IsDeleted = true;
            //await _unitOfWork.CompleteAsync();

            productEntity.UpdatedBy=_userService.GetCurrentUserId();
            productEntity.Updated = DateTime.Now;
            await _productRepository.UpdateAsync(productEntity);

            return ResultView<bool>.Success(true);
        }

        public async Task<EntityPaginatedB<ProductDto>> GetAllPaginatedByLanguageAsync(int pageNumber, int count, int languageId)
        {
            
            var productsPaginated = await _productRepository.GetAllPaginatedAsync(pageNumber, count);


            var productDtos = productsPaginated.Data.Select(product => new ProductDto
            {
                Id = product.Id,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CreatedBy = product.CreatedBy,
                UpdatedBy = product.UpdatedBy,
                IsDeleted = product.IsDeleted,

                Translations = _mapper.Map<List<ProductTranslationDto>>(
                    product.Translations.Where(t => t.LanguageId == languageId).ToList()
                ),


                Images = _mapper.Map<List<ProductImageDto>>(product.Images),



                Specifications = product.Specifications.Select(spec => new ProductSpecificationDto
                {


                    Translations = _mapper.Map<List<ProductSpecificationTranslationDto>>(
                        spec.Translations.Where(t => t.LanguageId == languageId).ToList()
                    )
                }).ToList()
            });

            // Return the paginated DTOs with pagination metadata
            return new EntityPaginatedB<ProductDto>
            {
                Data = productDtos,
                PageNumber = productsPaginated.PageNumber,
                Count = productsPaginated.Count,
                CountAllItems = productsPaginated.CountAllItems
            };
        }


        
        // Handling Related Entities

        //public async Task<ResultView<ProductDto>> AddImagesToProductAsync(int productId, IEnumerable<ProductImageDto> imageDtos)
        //{
        //    var images = _mapper.Map<IEnumerable<ProductImageB>>(imageDtos);
        //    await _productRepository.AddImagesAsync(productId, images);
        //    return await GetProductByIdAsync(productId); // Return updated product
        //}

        //public async Task<ResultView<ProductDto>> RemoveImageFromProductAsync(int productId, int imageId)
        //{
        //    await _productRepository.RemoveImageAsync(productId, imageId);
        //    return await GetProductByIdAsync(productId); // Return updated product
        //}

        //public async Task<ResultView<ProductDto>> AddTranslationsToProductAsync(int productId, IEnumerable<ProductTranslationDto> translationDtos)
        //{
        //    var translations = _mapper.Map<IEnumerable<ProductTranslationB>>(translationDtos);
        //    await _productRepository.AddTranslationsAsync(productId, translations);
        //    return await GetProductByIdAsync(productId); // Return updated product
        //}

        //public async Task<ResultView<ProductDto>> RemoveTranslationFromProductAsync(int productId, int translationId)
        //{
        //    await _productRepository.RemoveTranslationAsync(productId, translationId);
        //    return await GetProductByIdAsync(productId); // Return updated product
        //}

        //public async Task<ResultView<ProductDto>> AddSpecificationsToProductAsync(int productId, IEnumerable<ProductSpecificationsDto> specificationDtos)
        //{
        //    var specifications = _mapper.Map<IEnumerable<ProductSpecificationsB>>(specificationDtos);
        //    await _productRepository.AddSpecificationsAsync(productId, specifications);
        //    return await GetProductByIdAsync(productId); // Return updated product
        //}

        //public async Task<ResultView<ProductDto>> RemoveSpecificationFromProductAsync(int productId, int specificationId)
        //{
        //    await _productRepository.RemoveSpecificationAsync(productId, specificationId);
        //    return await GetProductByIdAsync(productId); // Return updated product
        //}

        //public IQueryable<ProductSpecificationsB> GetSpecificationsByProductId(int productId)
        //{
        //    return _productRepository.GetSpecificationsByProductId(productId);
        //}
    }

}

/*
  private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<ResultView<ProductDto>> CreateProductAsync(ProductDto productDto, int userId)
    {
        var product = _mapper.Map<ProductB>(productDto);
        product.CreatedBy = userId;

        // Handle translations
        if (productDto.Translations != null)
        {
            product.Translations = productDto.Translations.Select(t => new ProductTranslationB
            {
                Name = t.Name,
                BrandName = t.BrandName,
                Description = t.Description
            }).ToList();
        }

        // Handle specifications
        if (productDto.Specifications != null)
        {
            product.Specifications = productDto.Specifications.Select(spec => new ProductSpecificationsB
            {
                Translations = spec.Translations.Select(trans => new ProductSpecificationTranslationB
                {
                    TranslatedKey = trans.TranslatedKey,
                    TranslatedValue = trans.TranslatedValue
                }).ToList()
            }).ToList();
        }

        await _productRepository.AddAsync(product);

        return new ResultView<ProductDto>
        {
            Success = true,
            Data = productDto
        };
    }

    public async Task<ResultView<ProductDto>> UpdateProductAsync(ProductDto productDto, int userId)
    {
        var product = await _productRepository.GetByIdAsync(productDto.Id);
        if (product == null)
        {
            return new ResultView<ProductDto>
            {
                Success = false,
                Message = "Product not found."
            };
        }

        // Update basic properties
        product.UpdatedBy = userId;
        product.Updated = DateTime.Now;

        // Update translations
        product.Translations = productDto.Translations.Select(t => new ProductTranslationB
        {
            Name = t.Name,
            BrandName = t.BrandName,
            Description = t.Description
        }).ToList();

        // Update specifications
        product.Specifications = productDto.Specifications.Select(spec => new ProductSpecificationsB
        {
            Translations = spec.Translations.Select(trans => new ProductSpecificationTranslationB
            {
                TranslatedKey = trans.TranslatedKey,
                TranslatedValue = trans.TranslatedValue
            }).ToList()
        }).ToList();

        await _productRepository.UpdateAsync(product);

        return new ResultView<ProductDto>
        {
            Success = true,
            Data = productDto
        };
    }
}
 
 
 */