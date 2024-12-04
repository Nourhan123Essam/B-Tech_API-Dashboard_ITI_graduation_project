using ApplicationB.Contracts_B.Category;
using ApplicationB.Services_B.Category;
using ApplicationB.Services_B.General;
using AutoMapper;
using DTOsB.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DTOsB.Controllers
{

    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ILanguageService _languageService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, ILanguageService languageService, IMapper mapper, ICategoryRepository categoryRepository)
        {
            _categoryService = categoryService;
            _languageService = languageService;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _categoryService.GetAllCategoriesAsync();

            if (!result.IsSuccess)
            {
                return View("Error", result.Msg);
            }

            var categories = result.Entity;

            ViewBag.Languages = await _languageService.GetAllLanguagesAsync();
            return View(categories);
        }
        public async Task<IActionResult> Search(string name)
        {
            //var categories = await _categoryService.GetCategoryByNameAsync(name);
            //ViewBag.Languages = await _languageService.GetAllLanguagesAsync(); // Populate languages
            //return View("Index", categories);
            var result = await _categoryService.GetCategoryByNameAsync(name);

            if (!result.IsSuccess)
            {
                ViewBag.ErrorMessage = result.Msg;
                ViewBag.Languages = await _languageService.GetAllLanguagesAsync();
                return View("Index", new List<GetAllCategoriesDTO>());
            }

            var categories = result.Entity;
            ViewBag.Languages = await _languageService.GetAllLanguagesAsync();
            return View("Index", categories);
        }
        public async Task<IActionResult> FilterByLanguage(int languageId)
        {
            IEnumerable<GetAllCategoriesDTO> categories;

            if (languageId > 0)
            {
                var result = await _categoryService.GetCategoriesByLanguageAsync(languageId);
                if (!result.IsSuccess)
                {
                    return NotFound(result.Msg);
                }
                categories = result.Entity;
            }
            else
            {
                var allCategoriesResult = await _categoryService.GetAllCategoriesAsync();
                if (!allCategoriesResult.IsSuccess)
                {
                    return NotFound(allCategoriesResult.Msg);
                }
                categories = allCategoriesResult.Entity;
            }

            ViewBag.Languages = await _languageService.GetAllLanguagesAsync();
            return View("Index", categories);
        }


        public async Task<IActionResult> Create()
        {
            // Get all languages for the dropdown list
            var languages = await _languageService.GetAllLanguagesAsync();
            ViewBag.Languages = languages.Select(l => new SelectListItem
            {

                Value = l.Id.ToString(),
                Text = l.Name
            }).ToList();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrUpdateCategoriesDTO model, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    // Check if translations are added
                    if (model.Translations == null || !model.Translations.Any())
                    {
                        throw new Exception("At least one translation is required.");
                    }

                    await _categoryService.AddCategoryAsync(model, imageFile);
                    return RedirectToAction("Index", "Category"); // Redirect to the category list
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            //if (!ModelState.IsValid)
            //{
            //    foreach (var entry in ModelState)
            //    {
            //        var errors = entry.Value.Errors;
            //        foreach (var error in errors)
            //        {

            //            Console.WriteLine($"Error in {entry.Key}: {error.ErrorMessage}");
            //        }
            //    }
            //}
            // Reload the languages if the model state is invalid
            var languages = await _languageService.GetAllLanguagesAsync();
            ViewBag.Languages = languages.Select(l => new SelectListItem
            {
                Value = l.Id.ToString(),
                Text = l.Name
            }).ToList();


            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (!category.IsSuccess || category.Entity == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<CreateOrUpdateCategoriesDTO>(category.Entity);

            // Initialize Translations if none exist
            if (model.Translations == null || !model.Translations.Any())
            {
                model.Translations = new List<CreateCategoryTranslationDto>
                {
                    new CreateCategoryTranslationDto()
                };
            }

            var languages = await _languageService.GetAllLanguagesAsync();
            if (languages == null || !languages.Any())
            {
                ModelState.AddModelError("", "No languages found.");
            }
            ViewBag.Languages = new SelectList(languages, "Id", "Name");


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateOrUpdateCategoriesDTO model, IFormFile imageFile)
        {
            //if (ModelState.IsValid)
            //{
                try
                {
                    //model.Translations[0].IsMainCategory = Request.Form["Translations[0].IsMainCategory"] == "true";

                    await _categoryService.UpdateCategoryAsync(id, model, imageFile);
                    return RedirectToAction("Index", "Category");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating category: {ex.Message}");

                    ModelState.AddModelError("", ex.Message);
                }
            //}

            var languages = await _languageService.GetAllLanguagesAsync();
            ViewBag.Languages = new SelectList(languages, "Id", "Name");

            return View(model);
        }

       
        public async Task<IActionResult> Delete(int id)
        {
            var categoryEntity = await _categoryService.GetCategoryByIdAsync(id);
            if (categoryEntity == null)
            {
                return NotFound();
            }
            var categoryDto = _mapper.Map<GetAllCategoriesDTO>(categoryEntity.Entity);

            return View(categoryDto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //await _categoryService.DeleteCategoryAsync(id);
            //return RedirectToAction(nameof(Index));
            var result = await _categoryService.DeleteCategoryAsync(id);

            if (!result.IsSuccess)
            {
                // Optionally, return the user back to the delete view with an error message
                TempData["ErrorMessage"] = result.Msg;
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
