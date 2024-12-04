using ApplicationB.Contracts_B.Product;
using ApplicationB.Services_B.User;
using AutoMapper;
using DTOsB.Product;
using DTOsB.Shared;
using Microsoft.EntityFrameworkCore;
using ModelsB.Product_B;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationB.Services_B.Product
{
    public class ProductImageService : IProductImageService
    {
        private readonly IProductImageRepository _productImageRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public ProductImageService(IProductImageRepository productImageRepository, IMapper mapper, IUserService userService)
        {
            _productImageRepository = productImageRepository;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<ResultView<ProductImageCreateOrUpdateDto>> AddImageAsync(ProductImageCreateOrUpdateDto productImageDto)
        {
            var productImage = _mapper.Map<ProductImageB>(productImageDto);
          

            await _productImageRepository.AddAsync(productImage);
            return ResultView<ProductImageCreateOrUpdateDto>.Success(productImageDto);
          
        }

        public async Task<ResultView<ProductImageCreateOrUpdateDto>> UpdateImageAsync(ProductImageCreateOrUpdateDto productImageDto)
        {
            var existingImage = await _productImageRepository.GetByIdAsync(productImageDto.Id);
            if (existingImage == null || existingImage.Product.IsDeleted)
            {
                return ResultView<ProductImageCreateOrUpdateDto>.Failure("Image not found.");
              
            }
            _mapper.Map(productImageDto, existingImage);

            await _productImageRepository.UpdateAsync(existingImage);
            return ResultView<ProductImageCreateOrUpdateDto>.Success(productImageDto);
       
        }


        //No need with Product Soft delete
        //public async Task<ResultView<ProductImageDto>> DeleteImageAsync(int id)
        //{
        //    await _productImageRepository.DeleteAsync(id);

        //    return ResultView<ProductImageDto>.Success(null);
        //}

        public async Task<ResultView<ProductImageDto>> GetProductImageByIdAsync(int id)
        {
            var productImage = await _productImageRepository.GetByIdAsync(id);
            if (productImage == null)
            {
                return ResultView<ProductImageDto>.Failure("Product image not found.");
            }

            var productImageDto = _mapper.Map<ProductImageDto>(productImage);
            return ResultView<ProductImageDto>.Success(productImageDto);
        }

        public async Task<ResultView<List<ProductImageDto>>> GetProductImagesByProductIdAsync(int productId)
        {
           
            var productImages = await _productImageRepository.GetImagesByProductId(productId).ToListAsync();
            var productImageDtos = _mapper.Map<List<ProductImageDto>>(productImages);

            return ResultView<List<ProductImageDto>>.Success(productImageDtos);
        }


    }
}
