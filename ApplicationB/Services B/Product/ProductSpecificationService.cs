using ApplicationB.Contracts_B.Product;
using ApplicationB.Services_B.General;
using AutoMapper;
using DTOsB.Product;
using DTOsB.Shared;
using ModelsB.Product_B;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationB.Services_B.Product
{
    public class ProductSpecificationService: IProductSpecificationService
    {
        private readonly IProductSpecificationRepository _specificationRepository;
        private readonly IMapper _mapper;


        public ProductSpecificationService(IProductSpecificationRepository specificationRepository, IMapper mapper,
             ILanguageService languageService)
        {
            _specificationRepository = specificationRepository;
            _mapper = mapper;

           
        }

        public async Task<ResultView<ProductSpecificationDto>> AddSpecificationAsync(ProductSpecificationDto specificationDto)
        {
            if (specificationDto.Id > 0)
                return ResultView<ProductSpecificationDto>.Failure("This Specifiction is already exists. Use update to modify it.");

            var specification = _mapper.Map<ProductSpecificationsB>(specificationDto);
           

            await _specificationRepository.AddAsync(specification);
            return ResultView<ProductSpecificationDto>.Success(specificationDto);
           
        }

        public async Task<ResultView<ProductSpecificationDto>> UpdateSpecificationAsync(ProductSpecificationDto specificationDto)
        {
            var existingSpec = await _specificationRepository.GetByIdAsync(specificationDto.Id);
            if (existingSpec == null || existingSpec.Product.IsDeleted)
            {
                return ResultView<ProductSpecificationDto>.Failure("Specification not found.");
              
            }

           
            _mapper.Map(specificationDto, existingSpec);
          
            await _specificationRepository.UpdateAsync(existingSpec);
            return ResultView<ProductSpecificationDto>.Success(specificationDto);
            
        }

        //no need to delete
        //public async Task<ResultView<ProductSpecificationDto>> DeleteSpecificationAsync(int id, int userId)
        //{
        //    await _specificationRepository.DeleteAsync(id);
        //    return  ResultView<ProductSpecificationDto>.Success(null);
        //}

        public async Task<ResultView<IEnumerable<ProductSpecificationDto>>> GetSpecificationsByProductIdAsync(int productId)
        {
            var specifications = await _specificationRepository.GetSpecificationsByProductId(productId);
            var specificationDtos = _mapper.Map<IEnumerable<ProductSpecificationDto>>(specifications);
            return ResultView<IEnumerable<ProductSpecificationDto>>.Success(specificationDtos);
        }

        public async Task<ResultView<ProductSpecificationDto>> GetSpecificationByIdAsync(int id)
        {
            var specification = await _specificationRepository.GetByIdAsync(id);
            if (specification == null || specification.Product.IsDeleted)
            {
                return ResultView<ProductSpecificationDto>.Failure("Specification not found.");
            }

            var specificationDto = _mapper.Map<ProductSpecificationDto>(specification);
            return ResultView<ProductSpecificationDto>.Success(specificationDto);
        }
    }
}
