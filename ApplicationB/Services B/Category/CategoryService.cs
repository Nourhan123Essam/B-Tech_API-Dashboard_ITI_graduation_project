using ApplicationB.Contracts_B.Category;
using ApplicationB.Contracts_B.General;
using ApplicationB.Services_B.General;
using ApplicationB.Services_B.User;
using AutoMapper;
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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ILanguageService _languageService;
        private readonly ILanguageRepository _languageRepository;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, IUserService userService, ILanguageService languageService, ILanguageRepository languageRepository)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _userService = userService;
            _languageService = languageService;
            _languageRepository = languageRepository;

        }

        public async Task<ResultView<IEnumerable<GetAllCategoriesDTO>>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var activeCategories = categories.Where(c => !c.IsDeleted);
            var category = _mapper.Map<IEnumerable<GetAllCategoriesDTO>>(activeCategories);
            return ResultView<IEnumerable<GetAllCategoriesDTO>>.Success(category);
        }
        public async Task<ResultView<IEnumerable<GetAllCategoriesDTO>>> GetCategoriesByLanguageAsync(int languageId)
        {
            var categories = await _categoryRepository.GetByLanguageAsync(languageId);

            if (categories == null || !categories.Any())
            {
                return ResultView<IEnumerable<GetAllCategoriesDTO>>.Failure("No categories found for the specified language.");
            }

            var mappedCategories = _mapper.Map<IEnumerable<GetAllCategoriesDTO>>(categories);
            return ResultView<IEnumerable<GetAllCategoriesDTO>>.Success(mappedCategories);
        }

        public async Task<ResultView<CategoryB>> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {

                return ResultView<CategoryB>.Failure($"Category with ID {id} not found.");
            }
            return ResultView<CategoryB>.Success(category);
        }
        public async Task<ResultView<IEnumerable<GetAllCategoriesDTO>>> GetCategoryByNameAsync(string categoryName)
        {

            // Ensure the input is valid
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                return ResultView<IEnumerable<GetAllCategoriesDTO>>.Failure("Category name cannot be empty.");
            }

            // Retrieve categories and filter by name
            var categories = await _categoryRepository.GetAllAsync();

            var filteredCategories = categories
                    .Where(c => c.Translations.Any(t =>
                        t.CategoryName.Contains(categoryName, StringComparison.OrdinalIgnoreCase)))
                    .Select(c => new CategoryB
                    {
                        Id = c.Id,
                        ImageUrl = c.ImageUrl,
                        CreatedBy = c.CreatedBy,
                        UpdatedBy = c.UpdatedBy,
                        // Keep only the translation matching the specified category name
                        Translations = c.Translations
                            .Where(t => t.CategoryName.Contains(categoryName, StringComparison.OrdinalIgnoreCase))
                            .ToList(),
                        ProductCategories = c.ProductCategories // Keep product associations if necessary
                    })
                    .ToList();
            var mappedCategories = _mapper.Map<IEnumerable<GetAllCategoriesDTO>>(filteredCategories);

            // Check if any categories were found
            if (!mappedCategories.Any())
            {
                return ResultView<IEnumerable<GetAllCategoriesDTO>>.Failure($"No categories found matching '{categoryName}'.");
            }

            return ResultView<IEnumerable<GetAllCategoriesDTO>>.Success(mappedCategories);

        }
        public async Task<ResultView<string>> AddCategoryAsync(CreateOrUpdateCategoriesDTO createCategoryDto, IFormFile imageFile)
        {
            // Check if a category with the same name already exists
            var existingCategory = await _categoryRepository
                .AnyAsync(c => c.Translations.Any(t => t.CategoryName.ToLower() == createCategoryDto.Translations.First().CategoryName.ToLower()));

            if (existingCategory)
            {
                return ResultView<string>.Failure("A category with the same name already exists.");
            }

            // Handle image upload
            string imageUrl = null;
            if (imageFile != null)
            {
                imageUrl = await HandleImageUploadAsync(imageFile);
                if (string.IsNullOrEmpty(imageUrl))
                {
                    return ResultView<string>.Failure("Image upload failed. Please provide a valid image.");
                }
            }

            // Get current user and language information
            var currentUserId = _userService.GetCurrentUserId();
            var currentLanguage = _languageService.GetCurrentLanguageCode();

            // Map DTO to entity and set metadata
            var category = _mapper.Map<CategoryB>(createCategoryDto);
            category.CreatedBy = currentUserId;
            category.UpdatedBy = currentUserId;
            category.ImageUrl = imageUrl;
            category.ProductCategories = new List<ProductCategoryB>();

            // Ensure translations exist
            if (createCategoryDto.Translations == null || !createCategoryDto.Translations.Any())
            {
                return ResultView<string>.Failure("Translations list cannot be null or empty.");
            }

            // Process each translation and associated product categories
            foreach (var translation in createCategoryDto.Translations)
            {
                if (translation == null)
                {
                    return ResultView<string>.Failure("Translation cannot be null.");
                }

                // Validate LanguageId and set default if needed
                if (translation.LanguageId == null || translation.LanguageId <= 0)
                {
                    translation.LanguageId = 2; // Default language ID if applicable
                }

                var languageExists = await _languageRepository.AnyAsync(l => l.Id == translation.LanguageId);
                if (!languageExists)
                {
                    return ResultView<string>.Failure($"Language with ID {translation.LanguageId} does not exist.");
                }

                // Validate the translation's CategoryName
                if (string.IsNullOrEmpty(translation.CategoryName))
                {
                    return ResultView<string>.Failure("Category name cannot be null or empty.");
                }

                // Associate products if provided
                //if (createCategoryDto.ProductIds != null && createCategoryDto.ProductIds.Any())
                //{
                //    foreach (var productId in createCategoryDto.ProductIds)
                //    {
                //        // Create ProductCategoryB entity
                //        var productCategoryEntity = new ProductCategoryB
                //        {
                //            ProductId = productId,
                //            Category = category,
                //            IsMainCategory = translation.IsMainCategory
                //        };
                //        category.ProductCategories.Add(productCategoryEntity);
                //    }
                //}
            }

            // Log category data to check before saving
            Console.WriteLine($"Category Data: {category}");

            // Save category to the database
            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();

            return ResultView<string>.Success("Category added successfully.");
        }

        public async Task<ResultView<string>> UpdateCategoryAsync(int id, CreateOrUpdateCategoriesDTO categoryDto, IFormFile imageFile)
        {
            var categoryEntity = await _categoryRepository.GetByIdAsync(id);
            if (categoryEntity == null)
            {
                return ResultView<string>.Failure($"Category with ID {id} not found.");
            }
            var existingImageUrl = categoryEntity.ImageUrl;

            // Handle image upload (convert IFormFile to a URL or save path)
            if (imageFile != null)
            {
                var newImageUrl = await HandleImageUploadAsync(imageFile);
                if (string.IsNullOrEmpty(newImageUrl))
                {
                    categoryEntity.ImageUrl = existingImageUrl;
                    return ResultView<string>.Failure("Image upload failed. Please provide a valid image.");
                }
                categoryEntity.ImageUrl = newImageUrl;
            }
            else
            {
                categoryEntity.ImageUrl = !string.IsNullOrEmpty(categoryDto.ImageUrl) ? categoryDto.ImageUrl : existingImageUrl;
            }



            // Update the user and updated date
            categoryEntity.UpdatedBy = _userService.GetCurrentUserId();
            categoryEntity.Updated = DateTime.Now;

            // Clear existing translations and product categories
            categoryEntity.Translations.Clear();
            categoryEntity.ProductCategories.Clear();

            // Validate and update translations using AutoMapper
            if (categoryDto.Translations == null || !categoryDto.Translations.Any())
            {
                return ResultView<string>.Failure("Translations list cannot be null or empty.");
            }
            // Update translations using AutoMapper
            foreach (var translationDto in categoryDto.Translations)
            {
                if (string.IsNullOrEmpty(translationDto.CategoryName))
                {
                    return ResultView<string>.Failure("Category name cannot be null or empty.");
                }

                // Map translation DTO to entity
                var translation = _mapper.Map<CategoryTranslationB>(translationDto);
                categoryEntity.Translations.Add(translation);

                // Add product category relationships
                //if (categoryDto.ProductIds != null && categoryDto.ProductIds.Any())
                //{
                //    foreach (var productId in categoryDto.ProductIds)
                //    {
                //        var productCategory = new ProductCategoryB
                //        {
                //            ProductId = productId,
                //            Category = categoryEntity,
                //            IsMainCategory = categoryDto.Translations.First().IsMainCategory // Assuming the main category flag is derived from the first translation
                //        };
                //        categoryEntity.ProductCategories.Add(productCategory);
                //    }
                //}
            }

            // Update the category in the repository
            await _categoryRepository.UpdateAsync(categoryEntity);

            // Save changes
            await _categoryRepository.SaveChangesAsync();
            return ResultView<string>.Success("Category updated successfully.");
        }


        public async Task<ResultView<string>> DeleteCategoryAsync(int id)
        {
            var categoryEntity = await _categoryRepository.GetByIdAsync(id);
            if (categoryEntity == null)
            {
                return ResultView<string>.Failure($"Category with ID {id} not found.");
            }
            // Check if the category is linked to any products or subcategories
            if (categoryEntity.ProductCategories.Any())
            {
                // You can choose to throw an exception, log a warning, or handle it accordingly
                return ResultView<string>.Failure($"Category with ID {id} cannot be deleted because it has associated products or subcategories.");
            }
            // soft delete
            categoryEntity.IsDeleted = true;
            categoryEntity.UpdatedBy = _userService.GetCurrentUserId(); // Log user who performed the action
            categoryEntity.Updated = DateTime.Now;

            await _categoryRepository.UpdateAsync(categoryEntity);
            await _categoryRepository.SaveChangesAsync();
            return ResultView<string>.Success("Category deleted successfully.");

        }

        public async Task<string> HandleImageUploadAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            // Logic to save the image to a folder and return the file path or URL
            var fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
            var fileExtension = Path.GetExtension(imageFile.FileName);
            var newFileName = $"{fileName}_{DateTime.Now.Ticks}{fileExtension}";

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageUrls/categories", newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
            Console.WriteLine($"Image saved to: {filePath}");

            return $"/ImageUrls/categories/{newFileName}"; // Return the relative URL or path        
        }

    }
}
