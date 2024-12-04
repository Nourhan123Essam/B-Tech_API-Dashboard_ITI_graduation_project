using DTOsB.Category;
using DTOsB.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationB.Services_B.Category
{
    public interface IProductCategoryService
    {
        Task<ResultView<IEnumerable<ProductCategoryDto>>> GetAllAsync();
        Task<ResultView<ProductCategoryDto>> GetByIdAsync(int productId, int categoryId);
        Task<ResultView<ProductCategoryDto>> AddAsync(ProductCategoryDto productCategoryDto);
        Task<ResultView<List<ProductCategoryDto>>> UpdateAsync(List<ProductCategoryDto> productCategoryDto);
        Task<ResultView<bool>> DeleteAsync(int productId, int categoryId);
        public Task<ResultView<List<ProductCategoryDto>>> GetProductsByCategoryNameAsync(string categoryName);
        public Task<ResultView<List<ProductCategoryDto>>> GetProductsByCategoryIdAsync(int categoryId);
        public Task<ResultView<List<ProductCategoryDto>>> GetMainCategoriesAsync();
        public Task<ResultView<List<ProductCategoryDto>>> GetSubCategoriesAsync();

        public Task<ResultView<IEnumerable<GetAllCategoriesDTO>>> GetSubCategoriesByMainCategoryIdAsync(int mainCategoryId);

        Task<ResultView<List<ProductCategoryDto>>> GetCategoriesByProductIdAsync(int productId);

        Task<ResultView<bool>> DeleteByProductIdAsync(int productId);

    }
}
