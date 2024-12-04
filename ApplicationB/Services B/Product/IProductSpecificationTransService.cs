using DTOsB.Product;
using DTOsB.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationB.Services_B.Product
{
    public interface IProductSpecificationTransService
    {
        Task<ResultView<IEnumerable<ProductSpecificationTranslationDto>>> GetTranslationsBySpecificationIdAsync(int specificationId);
        Task<ResultView<ProductSpecificationTranslationDto>> AddTranslationAsync(ProductSpecificationTranslationDto translationDto);
        Task<ResultView<ProductSpecificationTranslationDto>> UpdateTranslationAsync(ProductSpecificationTranslationDto translationDto);

        Task<ResultView<IEnumerable<ProductSpecificationTranslationDto>>> GetSpecificationsTransByProductIdAsync(int productId);
        Task<ResultView<ProductSpecificationTranslationDto>> GetSpecificationByIdAsync(int id);
    }
}
