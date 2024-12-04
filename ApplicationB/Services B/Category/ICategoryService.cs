using DTOsB.Category;
using DTOsB.Shared;
using Microsoft.AspNetCore.Http;
using ModelsB.Category_B;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationB.Services_B.Category
{
    public interface ICategoryService
    {
        public Task<ResultView<IEnumerable<GetAllCategoriesDTO>>> GetAllCategoriesAsync();
        public Task<ResultView<CategoryB>> GetCategoryByIdAsync(int id);
        public Task<ResultView<IEnumerable<GetAllCategoriesDTO>>> GetCategoryByNameAsync(string categoryName);
        public Task<ResultView<IEnumerable<GetAllCategoriesDTO>>> GetCategoriesByLanguageAsync(int languageId);

        public Task<ResultView<string>> AddCategoryAsync(CreateOrUpdateCategoriesDTO createCategoryDto, IFormFile imageFile);
        public Task<ResultView<string>> UpdateCategoryAsync(int id, CreateOrUpdateCategoriesDTO categoryDto, IFormFile imageFile);
        public Task<ResultView<string>> DeleteCategoryAsync(int id);
        public Task<string> HandleImageUploadAsync(IFormFile imageFile);
    }
}
