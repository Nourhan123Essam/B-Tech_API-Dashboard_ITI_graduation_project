using ApplicationB.Contracts_B.Category;
using ApplicationB.Contracts_B;
using DbContextB;
using ModelsB.Category_B;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using DTOsB.Category;
using ModelsB.Product_B;

namespace InfrastructureB.Category
{
    public class ProductCategoryRepository : IGenericRepositoryB<ProductCategoryB>, IProductCategoryRepository
    {

            private readonly BTechDbContext _dbContext;

            public ProductCategoryRepository(BTechDbContext dbContextB)
            {
                _dbContext = dbContextB;
            }

            public async Task AddAsync(ProductCategoryB productCategory)
            {
                await _dbContext.ProductCategories.AddAsync(productCategory);
                await _dbContext.SaveChangesAsync();
            }

            public async Task DeleteAsync(int productId, int categoryId)
            {
                var productCategory = await GetByIdAsync(productId, categoryId);
                if (productCategory != null)
                {
                    _dbContext.ProductCategories.Remove(productCategory);
                    await _dbContext.SaveChangesAsync();
                }
            }


            public async Task<bool> ExistsAsync(int productId, int categoryId)
            {
                return await _dbContext.ProductCategories.AnyAsync(pc => pc.ProductId == productId && pc.CategoryId == categoryId);
            }

            public async Task<IEnumerable<ProductCategoryB>> GetAllAsync()
            {
                return await _dbContext.ProductCategories
                           .Include(pc => pc.Product)
                           .Include(pc => pc.Category)
                           .ToListAsync();
            }

            public async Task<ProductCategoryB> GetByIdAsync(int productId, int categoryId)
            {
                return await _dbContext.ProductCategories
                           .Where(pc => pc.ProductId == productId && pc.CategoryId == categoryId)
                           .Include(pc => pc.Product)
                           .Include(pc => pc.Category)
                           .FirstOrDefaultAsync();
            }

        public async Task<List<ProductCategoryB>> GetByProductIdAsync(int productId)
        {
            return await _dbContext.ProductCategories
                       .Where(pc => pc.ProductId == productId)
                       .Include(pc => pc.Product)
                       .Include(pc => pc.Category)
                       .ToListAsync();
        }


        public async Task UpdateAsync(ProductCategoryB productCategory)
            {
                _dbContext.ProductCategories.Update(productCategory);
                await _dbContext.SaveChangesAsync();
            }

            public async Task<IEnumerable<ProductCategoryB>> GetByCategoryNameAsync(string categoryName)
            {
           
                return await _dbContext.ProductCategories
                              .Include(pc => pc.Product)
                                  .ThenInclude(p => p.Images) // Include images
                              .Include(pc => pc.Product)
                                  .ThenInclude(p => p.Translations) // Include translations for the product
                              .Include(pc => pc.Product)
                                  .ThenInclude(p => p.Specifications) // Include specifications
                                      .ThenInclude(s => s.Translations) // Include translations for specifications
                              .Include(pc => pc.Category)
                                  .ThenInclude(c => c.Translations) // Include translations for the category
                              .Where(pc => pc.Category.Translations.Any(t => t.CategoryName.ToLower() == categoryName.ToLower()))
                              .ToListAsync();
            }

            public async Task<IEnumerable<ProductCategoryB>> GetByCategoryIdAsync(int categoryId)
            {
              
                return await _dbContext.ProductCategories
                           .Include(pc => pc.Product.Images) // Include related images
                           .Include(pc => pc.Product.Translations) // Include related translations
                           .Include(pc => pc.Product.Specifications) // Include related specifications
                                 .ThenInclude(spec => spec.Translations) // Include translations for specifications
                           .Include(pc => pc.Category)
                                 .ThenInclude(c => c.Translations)
                           .Where(pc => pc.CategoryId == categoryId)
                           .ToListAsync();
            }

            public async Task<IEnumerable<ProductCategoryB>> GetMainCategoriesAsync()
            {
                return await _dbContext.ProductCategories
                    //.Include(pc => pc.Product)
                    .Include(pc => pc.Category)
                        .ThenInclude(c => c.Translations)
                    .Where(pc => pc.IsMainCategory)
                    .ToListAsync();
            }
            public async Task<List<ProductCategoryB>> GetSubCategoriesAsync()
            {
                return await _dbContext.ProductCategories
                    .Include(pc => pc.Category)
                        .ThenInclude(c => c.Translations)
                    //.Include(pc => pc.Product)
                    .Where(pc => !pc.IsMainCategory)
                    .ToListAsync();
            }
            public async Task<List<MainCategoryWithSubCategoriesDTO>> GetSubCategoriesByMainCategoryIdAsync(int mainCategoryId)
            {
            
                return await _dbContext.ProductCategories
                   .Where(pc => pc.IsMainCategory && pc.CategoryId == mainCategoryId) // 1. Filter to get only the specified main category
                   .Select(mainCategory => new MainCategoryWithSubCategoriesDTO
                   {
                       MainCategory = mainCategory.Category, // 2. Select the main category
                       SubCategories = _dbContext.ProductCategories
                           .Where(pc => !pc.IsMainCategory && pc.ProductId == mainCategory.ProductId) // 3. Get subcategories for the same product
                           .Include(pc => pc.Category) // Include the related category details
                                  .ThenInclude(c => c.Translations) // Include translations if required
                           .Select(pc => pc.Category) // 4. Select only the Category part
                           .ToList() // Convert subcategories to a list
                   })
                   .ToListAsync(); // Finally, convert the main categories with their subcategories into a list asynchronously
            }

        Task<IQueryable<ProductCategoryB>> IGenericRepositoryB<ProductCategoryB>.GetAllAsync()
            {
                throw new NotImplementedException();
            }
            public Task<ProductCategoryB> GetByIdAsync(int id)
            {
                throw new NotImplementedException();
            }
            public async Task DeleteAsync(int id)
            {
            var productCategory = await GetByProductIdAsync(id);
            if (productCategory != null)
            {
                foreach (var item in productCategory)
                {
                    _dbContext.ProductCategories.Remove(item);
                    await _dbContext.SaveChangesAsync();
                }
               
            }
        }
        public async Task<IEnumerable<ProductCategoryB>> GetCategoriesByProductIdAsync(int productId)
        {
            return await _dbContext.ProductCategories
                .Include(pc => pc.Product)
                .Include(pc => pc.Category)
                .ThenInclude(p => p.Translations)
                .Where(pc => pc.ProductId == productId)
                .ToListAsync();
        }

        Task<ProductCategoryB> IGenericRepositoryB<ProductCategoryB>.AddAsync(ProductCategoryB entity)
        {
            throw new NotImplementedException();
        }
    }
}

    

