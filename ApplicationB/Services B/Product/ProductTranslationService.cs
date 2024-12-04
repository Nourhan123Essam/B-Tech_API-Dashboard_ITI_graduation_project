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
    public class ProductTranslationService: IProductTranslationService
    {
        private readonly IProductTranslationRepository _translationRepository;
        private readonly IMapper _mapper;
        
        private readonly ILanguageService _languageService;

        public ProductTranslationService(IProductTranslationRepository translationRepository, IMapper mapper,
            IUserService userService, ILanguageService languageService)
        {
            _translationRepository = translationRepository;
            _mapper = mapper;
           
            _languageService = languageService;
        }

        public async Task<ResultView<ProductTranslationDto>> AddTranslationAsync(ProductTranslationDto translationDto)
        {
            var translation = _mapper.Map<ProductTranslationB>(translationDto);
            
           


            await _translationRepository.AddAsync(translation);
            return ResultView<ProductTranslationDto>.Success(translationDto);
      
        }

        public async Task<ResultView<ProductTranslationDto>> UpdateTranslationAsync(ProductTranslationDto translationDto)
        {
            var existingTranslation = await _translationRepository.GetByIdAsync(translationDto.Id);
            if (existingTranslation == null || existingTranslation.Product.IsDeleted)
            {
                return ResultView<ProductTranslationDto>.Failure("Translation not found.");
              
            }
            _mapper.Map(translationDto, existingTranslation);

            
            //existingTranslation.Language.Id = _languageService.GetCurrentLanguageCode();

            await _translationRepository.UpdateAsync(existingTranslation);
            return ResultView<ProductTranslationDto>.Success(translationDto);
           
        }



        public async Task<ResultView<IEnumerable<ProductTranslationDto>>> GetTranslationsByProductIdAsync(int productId)
        {
            int language = _languageService.GetCurrentLanguageCode();
            var translations = await _translationRepository.GetTranslationsByProductId(productId, language);
            if (!translations.Any())
            {
                return ResultView<IEnumerable<ProductTranslationDto>>.Failure("No translations found for this product.");
            }

            var translationDtos = _mapper.Map<IEnumerable<ProductTranslationDto>>(translations);
            return ResultView<IEnumerable<ProductTranslationDto>>.Success(translationDtos);
        }

        public async Task<ResultView<IEnumerable<ProductTranslationDto>>> GetAllTranslationsAsync()
        {
            var translations = await _translationRepository.GetAllAsync();
            var translationDtos = _mapper.Map<IQueryable<ProductTranslationDto>>(translations);
            return ResultView<IEnumerable<ProductTranslationDto>>.Success(translationDtos);
        }


        //No need to delete
        //public async Task<ResultView<ProductTranslationDto>> DeleteTranslationAsync(int id)
        //{
        //    var translation = await _translationRepository.GetByIdAsync(id);

        //    if (translation == null)
        //    {
        //        return ResultView<ProductTranslationDto>.Failure("Translation not found.");
        //    }

        //    if (translation.Product.IsDeleted == false)
        //    {
        //        return ResultView<ProductTranslationDto>.Failure("You Must Delete Product First");
        //    }
        //    return ResultView<ProductTranslationDto>.Success(null);
        //}
    }
}
