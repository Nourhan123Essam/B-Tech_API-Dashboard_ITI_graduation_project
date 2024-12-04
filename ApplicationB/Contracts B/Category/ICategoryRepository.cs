using ModelsB.Category_B;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationB.Contracts_B.Category
{
    public interface ICategoryRepository:IGenericRepositoryB<CategoryB>
    {
        Task<IEnumerable<CategoryB>> GetAllAsync();
        Task<CategoryB> GetByIdAsync(int id);
        Task<IEnumerable<CategoryB>> GetByNameAsync(string categoryName);
        Task<IEnumerable<CategoryB>> GetByLanguageAsync(int languageId);

        Task AddAsync(CategoryB category);
        Task UpdateAsync(CategoryB category);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
        Task<bool> AnyAsync(Expression<Func<CategoryB, bool>> predicate);

    }
}
