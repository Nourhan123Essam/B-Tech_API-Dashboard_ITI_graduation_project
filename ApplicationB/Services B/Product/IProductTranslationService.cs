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
    public interface IProductTranslationService
    {
        Task<ResultView<ProductTranslationDto>> AddTranslationAsync(ProductTranslationDto translationDto);
        Task<ResultView<ProductTranslationDto>> UpdateTranslationAsync(ProductTranslationDto translationDto);
        //Task<ResultView<ProductTranslationDto>> DeleteTranslationAsync(int id);
      
        Task<ResultView<IEnumerable<ProductTranslationDto>>> GetTranslationsByProductIdAsync(int productId);

        Task<ResultView<IEnumerable<ProductTranslationDto>>> GetAllTranslationsAsync();
    }
}
