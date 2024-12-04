using DTOsB.Category;
using ModelsB.Category_B;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationB.Contracts_B.Category
{
    public interface IProductCategoryRepository : IGenericRepositoryB<ProductCategoryB>
    {
        Task<IEnumerable<ProductCategoryB>> GetAllAsync();
        Task<ProductCategoryB> GetByIdAsync(int productId, int categoryId);
        Task AddAsync(ProductCategoryB productCategory);
        Task UpdateAsync(ProductCategoryB productCategory);
        Task DeleteAsync(int productId, int categoryId);
        Task<bool> ExistsAsync(int productId, int categoryId);
        Task<IEnumerable<ProductCategoryB>> GetByCategoryNameAsync(string categoryName);
        Task<IEnumerable<ProductCategoryB>> GetByCategoryIdAsync(int categoryId);
        Task<IEnumerable<ProductCategoryB>> GetMainCategoriesAsync();
        Task<List<ProductCategoryB>> GetSubCategoriesAsync();
        Task<List<MainCategoryWithSubCategoriesDTO>> GetSubCategoriesByMainCategoryIdAsync(int mainCategoryId);
        Task<IEnumerable<ProductCategoryB>> GetCategoriesByProductIdAsync(int productId);
        Task<List<ProductCategoryB>> GetByProductIdAsync(int productId);
    }
}
