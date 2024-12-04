using DTOsB.Product;
using DTOsB.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationB.Services_B.Product
{
    public interface IProductService
    {
        Task<ResultView<ProductCreateOrUpdateDto>> CreateProductAsync(ProductCreateOrUpdateDto productDto);
        Task<ResultView<ProductCreateOrUpdateDto>> UpdateProductAsync(ProductCreateOrUpdateDto productDto);
        Task<ResultView<ProductDto>> DeleteProductAsync(int id);
        Task<ResultView<ProductCreateOrUpdateDto>> GetProductByIdAsync(int id);
        Task<ResultView<IEnumerable<ProductDto>>> GetAllProductsAsync();
        Task<ResultView<IEnumerable<ProductDto>>> SearchProductsByNameAsync(string name);
        public Task<EntityPaginatedB<ProductDto>> GetAllPaginatedAsync(int pageNumber, int Count);
        Task<ResultView<bool>> DeleteProductModelAsync(int productId);

        Task<EntityPaginatedB<ProductDto>> GetAllPaginatedByLanguageAsync(int pageNumber, int count, int languageId);
    }
}
