using ApplicationB.Contracts_B.Product;
using ApplicationB.Services_B.User;
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
    public class SpecificationStoreService:ISpecificationStoreService
    {
        private readonly ISpecificationStoreRepository _specsStoreRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public SpecificationStoreService(ISpecificationStoreRepository specsStoreRepository,
            IMapper mapper, IUserService userService)
        {
            _specsStoreRepository = specsStoreRepository;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<ResultView<SpecificationStoreDto>> AddReviewAsync(SpecificationStoreDto SpecDto)
        {
            var specKey = _mapper.Map<SpecificationStore>(SpecDto);
          

            await _specsStoreRepository.AddAsync(specKey);
            return ResultView<SpecificationStoreDto>.Success(SpecDto);
        }

        public async Task<ResultView<SpecificationStoreDto>> UpdateReviewAsync(SpecificationStoreDto SpecDto)
        {
            var existingSpecKey = await _specsStoreRepository.GetByIdAsync(SpecDto.Id);
            if (existingSpecKey == null)
                return ResultView<SpecificationStoreDto>.Failure("Review not found.");

            _mapper.Map(SpecDto, existingSpecKey);
   

            await _specsStoreRepository.UpdateAsync(existingSpecKey);
            return ResultView<SpecificationStoreDto>.Success(SpecDto);
        }

        public async Task<ResultView<SpecificationStoreDto>> DeleteReviewAsync(int id)
        {
            var specKey = await _specsStoreRepository.GetByIdAsync(id);
            if (specKey == null)
                return ResultView<SpecificationStoreDto>.Failure("Review not found.");

            await _specsStoreRepository.DeleteAsync(specKey.Id);
            return ResultView<SpecificationStoreDto>.Success(null);
        }

        public async Task<ResultView<SpecificationStoreDto>> GetReviewByIdAsync(int id)
        {
            var specKey = await _specsStoreRepository.GetByIdAsync(id);
            if (specKey == null)
                return ResultView<SpecificationStoreDto>.Failure("Review not found.");

            var specKeyDto = _mapper.Map<SpecificationStoreDto>(specKey);
            return ResultView<SpecificationStoreDto>.Success(specKeyDto);
        }

        public async Task<IEnumerable<SpecificationStoreDto>> GetAllReviewsAsync()
        {
            var specs = await _specsStoreRepository.GetAllAsync();
            var specKeysDtos = _mapper.Map<IEnumerable<SpecificationStoreDto>>(specs);
            return specKeysDtos;
        }
    }
}

