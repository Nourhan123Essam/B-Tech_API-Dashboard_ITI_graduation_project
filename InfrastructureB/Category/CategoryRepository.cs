using ApplicationB.Contracts_B.Category;
using DbContextB;
using InfrastructureB.General;
using Microsoft.EntityFrameworkCore;
using ModelsB.Category_B;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureB.Category
{
    public class CategoryRepository : GenericRepositoryB<CategoryB>, ICategoryRepository
    {
        private readonly BTechDbContext _dbContext;
        public CategoryRepository(BTechDbContext dbContextB) : base(dbContextB)
        {
            _dbContext = dbContextB;
        }

        public async Task<IEnumerable<CategoryB>> GetByNameAsync(string categoryName)
        {
            return await _dbContext.Categories
                                 .Include(c => c.Translations)
                                 .Where(c => c.Translations.Any(t => t.CategoryName.ToLower().Equals(categoryName.ToLower())))
                                 .ToListAsync();
        }
        public async Task<IEnumerable<CategoryB>> GetByLanguageAsync(int languageId)
        {
            //return await _dbContext.Categories
            //                    .Where(c => c.Translations.Any(t => t.LanguageId == languageId))
            //                    .Include(c => c.Translations)
            //                    .ToListAsync();
            return await _context.Categories
                         .Where(c => c.Translations.Any(t => t.LanguageId == languageId))  // جلب التصنيفات التي تحتوي على الترجمة للغة المطلوبة
                         .Select(c => new CategoryB
                         {
                             Id = c.Id,
                             ImageUrl = c.ImageUrl,
                             CreatedBy = c.CreatedBy,
                             UpdatedBy = c.UpdatedBy,
                             // تعيين الترجمات المرتبطة فقط باللغة المطلوبة
                             Translations = c.Translations
                                 .Where(t => t.LanguageId == languageId)
                                 .ToList(),
                             ProductCategories = c.ProductCategories // الاحتفاظ بالربط مع المنتجات إن وجد
                         })
                         .ToListAsync();
        }
        public async Task AddAsync(CategoryB category)
        {
            await _dbContext.Categories.AddAsync(category);
        }

        public async Task DeleteAsync(int id)
        {
            var category = await GetByIdAsync(id);
            if (category != null)
            {
                _dbContext.Categories.Remove(category);
            }
        }

        public async Task<IEnumerable<CategoryB>> GetAllAsync()
        {

            return await _dbContext.Categories
                                     .Include(c => c.Translations)
                                    .ThenInclude(t => t.Language)
                                     .Include(c => c.ProductCategories)
                                    .Where(c => !c.IsDeleted)
                                     .ToListAsync();
        }

        public async Task<CategoryB> GetByIdAsync(int id)
        {
            return await _dbContext.Categories
                                 .Include(c => c.Translations)
                                 .Include(c => c.ProductCategories)
                                 .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(CategoryB category)
        {

            //_dbContext.Attach(category);
            //_dbContext.Entry(category).State = EntityState.Modified;
            _dbContext.Categories.Update(category);
            //await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<CategoryB, bool>> predicate)
        {
            return await _dbContext.Categories.AnyAsync(predicate);
        }
    }
}
