using ApplicationB.Contracts_B.Product;
using ApplicationB.Services_B.General;
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
    public class ProductSpecificationTransService: IProductSpecificationTransService
    {
        private readonly IProductSpecificationTranslationRepository _specTranslationRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ILanguageService _languageService;

        public ProductSpecificationTransService(IProductSpecificationTranslationRepository translationRepository,
            IMapper mapper,IUserService userService, ILanguageService languageService)
        {
            _specTranslationRepository = translationRepository;
            _mapper = mapper;
            _userService = userService;
            _languageService = languageService;
        }

        public async Task<ResultView<ProductSpecificationTranslationDto>> AddTranslationAsync
            (ProductSpecificationTranslationDto translationDto)
        {
            var translation = _mapper.Map<ProductSpecificationTranslationB>(translationDto);

            translation.ProductSpecification.Product.CreatedBy = _userService.GetCurrentUserId();
            translation.Language.Id = _languageService.GetCurrentLanguageCode();


            await _specTranslationRepository.AddAsync(translation);
            return ResultView<ProductSpecificationTranslationDto>.Success(translationDto);
        }

        public async Task<ResultView<IEnumerable<ProductSpecificationTranslationDto>>> GetSpecificationsTransByProductIdAsync(int productId)
        {
            int language = _languageService.GetCurrentLanguageCode();
            var translations = await _specTranslationRepository.GetTranslationsByProductId(productId,language);
            if (!translations.Any())
            {
                return ResultView<IEnumerable<ProductSpecificationTranslationDto>>.Failure("No Spec translations found for this product.");
            }

            var translationDtos = _mapper.Map<IEnumerable<ProductSpecificationTranslationDto>>(translations);
            return ResultView<IEnumerable<ProductSpecificationTranslationDto>>.Success(translationDtos);
        }



        public async Task<ResultView<IEnumerable<ProductSpecificationTranslationDto>>>
        GetTranslationsBySpecificationIdAsync(int specificationId)
        {
            var translations = await _specTranslationRepository.GetByIdAsync(specificationId);


            var translationDtos = _mapper.Map<IEnumerable<ProductSpecificationTranslationDto>>(translations);

            return ResultView<IEnumerable<ProductSpecificationTranslationDto>>.Success(translationDtos);
        }



        public async Task<ResultView<ProductSpecificationTranslationDto>>
            UpdateTranslationAsync(ProductSpecificationTranslationDto translationDto)
        {
            var existingTranslation = await _specTranslationRepository.GetByIdAsync(translationDto.Id);
            if (existingTranslation == null)
            {
                return ResultView<ProductSpecificationTranslationDto>.Failure("Translation not found.");
            }

         
            _mapper.Map(translationDto, existingTranslation);

            await _specTranslationRepository.UpdateAsync(existingTranslation);

            return ResultView<ProductSpecificationTranslationDto>.Success(translationDto);
        }

        public async Task<ResultView<ProductSpecificationTranslationDto>>
            GetSpecificationByIdAsync(int id)
        {
            var translation = await _specTranslationRepository.GetByIdAsync(id);
            if (translation == null)
            {
                return ResultView<ProductSpecificationTranslationDto>.Failure("Specification translation not found.");
            }

            var translationDto = _mapper.Map<ProductSpecificationTranslationDto>(translation);

            return ResultView<ProductSpecificationTranslationDto>.Success(translationDto);
        }
    }
}
