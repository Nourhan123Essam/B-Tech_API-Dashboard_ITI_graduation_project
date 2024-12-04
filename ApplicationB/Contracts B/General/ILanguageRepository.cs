using ModelsB.Localization_B;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationB.Contracts_B.General
{
    public interface ILanguageRepository
    {
        Task<LanguageB> GetByCodeAsync(string code);
        Task<LanguageB> GetByIdAsync(int Id);
        Task<List<LanguageB>> GetAllAsync();
        Task<bool> AnyAsync(Func<LanguageB, bool> predicate);
    }
}
