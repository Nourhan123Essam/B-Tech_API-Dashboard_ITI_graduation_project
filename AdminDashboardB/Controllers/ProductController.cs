using ApplicationB.Contracts_B.General;
using ApplicationB.Services_B;
using ApplicationB.Services_B.Category;
using ApplicationB.Services_B.General;
using ApplicationB.Services_B.Product;
using ApplicationB.Services_B.User;
using DTOsB.Category;
using DTOsB.Product;
using DTOsB.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ModelsB.Category_B;
using ModelsB.Localization_B;
using ModelsB.Product_B;
using PayPalCheckoutSdk.Orders;

namespace DTOsB.Controllers
{
    [Authorize(Roles = "Admin")]

    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IImageService _imageService;
        private readonly ILanguageService languageService;
        private readonly IUserService _userService;
        private readonly IProductService productService;
        private readonly IProductImageService _productImageService;
        private readonly IProductSpecificationService _productSpecificationService;
        private readonly IProductTranslationService _productTranslationService;
        private readonly IProductSpecificationTransService _productSpecificationTransService;
        private readonly ICategoryService categoryService;
        private readonly IProductCategoryService productCategoryService;


        public ProductController(IProductService _productService, IUserService userService, ILanguageService _languageService, IProductImageService productImageService, IProductSpecificationService productSpecificationService,
            IProductTranslationService productTranslationService, IProductSpecificationTransService productSpecificationTransService, IImageService imageService, ICategoryService _categoryService, IProductCategoryService _productCategoryService)
        {
            productService = _productService;
            _imageService = imageService;
            _userService = userService;
            languageService = _languageService;
            _productImageService = productImageService;
            _productSpecificationService = productSpecificationService;
            _productTranslationService = productTranslationService;
            _productSpecificationTransService = productSpecificationTransService;
            categoryService = _categoryService;
            productCategoryService = _productCategoryService;
        }


        public async Task<IActionResult> Index(string searchString, int? selectedLanguageId, int? selectedCategoryId)
        {
            ViewBag.SearchString = searchString;



            var availableLanguages = await languageService.GetAllLanguagesAsync();
            ViewBag.AvailableLanguages = availableLanguages;

            if (selectedLanguageId.HasValue && selectedLanguageId > 0)
            {
                await languageService.SetUserSelectedLanguageAsync((int)selectedLanguageId);
            }
            ViewBag.SelectedLanguageId = selectedLanguageId;

            var categories = await categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = categories.Entity;
            ViewBag.SelectedCategoryId = selectedCategoryId;

            var products = await productService.GetAllProductsAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                products = await productService.SearchProductsByNameAsync(searchString);
            }



            if (selectedCategoryId > 0)
            {
                var productsByCategory = productCategoryService.GetProductsByCategoryIdAsync((int)selectedCategoryId).Result.Entity;

                // Initialize _Products.Entity as IEnumerable<ProductDto>
                var _Products = new ResultView<IEnumerable<ProductDto>>()
                {
                    Entity = new List<ProductDto>() // Ensure Entity is initialized
                };

                foreach (var item in productsByCategory)
                {
                    _Products.Entity = _Products.Entity.Append(item.Product);
                }

                return View(_Products);
            }

            if (products == null)
            {
                // Handle null case for products
                return View("Error", "Products not available.");
            }
            return View(products);
        }


        //public async Task<IActionResult> Index(int pageNumber = 1, int count = 10, int selectedLanguageId = 2)
        //{
        //    var products = await productService.GetAllPaginatedByLanguageAsync(pageNumber, count, selectedLanguageId);
        //    var availableLanguages = await languageService.GetAllLanguagesAsync();

        //    var viewModel = new ProductViewModelWithLang
        //    {
        //        Products = products.Data,
        //        AvailableLanguages = availableLanguages,
        //        SelectedLanguageId = selectedLanguageId
        //    };

        //    return View(viewModel);
        //}

        //public async Task<IActionResult> Index()
        //{
        //    var products = await productService.GetAllProductsAsync();
        //    return View(products); // Assuming Data contains IEnumerable<ProductViewModel>
        //}

        // GET: Products/Create



        public async Task<IActionResult> Create()
        {

            var categories = await categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = categories.Entity;

            return View();
        }

        // POST: Products/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateOrUpdateDto productDto, List<GetAllCategoriesDTO> SelectedCategories)
        {
            //if (ModelState.IsValid)
            //{

            productDto.CreatedBy = _userService.GetAppUserByIdAsync(_userService.GetCurrentUserId()).Result.UserName;
            productDto.UpdatedBy = _userService.GetAppUserByIdAsync(_userService.GetCurrentUserId()).Result.UserName;



            var result = await productService.CreateProductAsync(productDto);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Msg);
                return View(productDto);
            }


            if (productDto.ImageFiles != null && productDto.ImageFiles.Count > 0)
            {

                result.Entity.Images = result.Entity.Images ?? new List<ProductImageCreateOrUpdateDto>();

                foreach (var imageFile in productDto.ImageFiles)
                {

                    var imageUrl = await _imageService.SaveImageAsync(imageFile, "ImageUrls");


                    var imageDto = new ProductImageCreateOrUpdateDto
                    {
                        Url = imageUrl,
                        ProductId = result.Entity.Id
                    };


                    var imageResult = await _productImageService.AddImageAsync(imageDto);
                    if (!imageResult.IsSuccess)
                    {
                        ModelState.AddModelError("", imageResult.Msg);
                        return View(result.Entity);
                    }
                }
            }


            for (int i = 0; i < SelectedCategories.Count; i++)
            {
                var newItem = new ProductCategoryDto
                {
                    CategoryId = SelectedCategories[i].Id,
                    ProductId = result.Entity.Id,
                    IsMainCategory = (i == 0) ? true : false,
                };
                var Result = await productCategoryService.AddAsync(newItem);

                if (!Result.IsSuccess)
                {
                    ModelState.AddModelError("", Result.Msg);
                    return View(result.Entity);
                }
            }

            //Translations
            //foreach (var trans in productDto.Translations)
            //{
            //    var transResult = await _productTranslationService.AddTranslationAsync(trans);
            //    if (!transResult.IsSuccess)
            //    {
            //        ModelState.AddModelError("", transResult.Msg);
            //        return View(productDto);
            //    }
            //}


            //Specifications
            //foreach (var spec in productDto.Specifications)
            //{
            //    var specResult = await _productSpecificationService.AddSpecificationAsync(spec);
            //    if (!specResult.IsSuccess)
            //    {
            //        ModelState.AddModelError("", specResult.Msg);
            //        return View(productDto);
            //    }

            //    foreach (var specTrans in spec.Translations)
            //    {
            //        var specTransResult = await _productSpecificationTransService.AddTranslationAsync(specTrans);
            //        if (!specTransResult.IsSuccess)
            //        {
            //            ModelState.AddModelError("", specTransResult.Msg);
            //            return View(productDto);
            //        }
            //    }
            //}

            //}
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var allCategories = await categoryService.GetAllCategoriesAsync();
            ViewBag.AllCategories = allCategories.Entity;

            var product = await productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Edit/5

        //[ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ResultView<ProductCreateOrUpdateDto> resultView,
            List<GetAllCategoriesDTO> SelectedCategories)
        {
            // Extract the actual DTO from the result view
            var productDto = resultView.Entity;

            if (id != productDto.Id)
            {
                return BadRequest();
            }

            //if (ModelState.IsValid)
            //{

            productDto.UpdatedBy = _userService.GetAppUserByIdAsync(_userService.GetCurrentUserId()).Result.UserName;
            productDto.CreatedBy = _userService.GetAppUserByIdAsync(_userService.GetCurrentUserId()).Result.UserName;


            List<ProductCategoryDto> Items = new List<ProductCategoryDto>();
            for (int i = 0; i < SelectedCategories.Count; i++)
            {

                var newItem = new ProductCategoryDto
                {
                    CategoryId = SelectedCategories[i].Id,
                    ProductId = productDto.Id,
                    IsMainCategory = (i == 0) ? true : false,
                };
                Items.Add(newItem);
                //await productCategoryService.DeleteByProductIdAsync(productDto.Id);
                //await productCategoryService.AddAsync(newItem);

            }
            await productCategoryService.UpdateAsync(Items);


            var result = await productService.UpdateProductAsync(productDto);
            var translations = productDto.Translations;
            foreach (var Trans in translations)
            {
                await _productTranslationService.UpdateTranslationAsync(Trans);
            }

            if (productDto.ImageFiles != null && productDto.ImageFiles.Count > 0)
            {

                result.Entity.Images = result.Entity.Images ?? new List<ProductImageCreateOrUpdateDto>();

                foreach (var imageFile in productDto.ImageFiles)
                {

                    var imageUrl = await _imageService.SaveImageAsync(imageFile, "ImageUrls");


                    var imageDto = new ProductImageCreateOrUpdateDto
                    {
                        Url = imageUrl,
                        ProductId = result.Entity.Id
                    };


                    var imageResult = await _productImageService.AddImageAsync(imageDto);
                    if (!imageResult.IsSuccess)
                    {
                        ModelState.AddModelError("", imageResult.Msg);
                        return View(result.Entity);
                    }
                }
            }
            productDto.Specifications = productDto.Specifications ?? new List<ProductSpecificationDto>();
            var specs = productDto.Specifications;
            foreach (var spec in specs)
            {
                // Retrieve the specification from the database based on spec.Id and update it
                var existingSpec = _productSpecificationService.GetSpecificationByIdAsync(spec.Id).Result.Entity;

                if (existingSpec != null)
                {
                    // Update the key and value for each language
                    var enTranslation = existingSpec.Translations.FirstOrDefault(t => t.LanguageId == 2);
                    var arTranslation = existingSpec.Translations.FirstOrDefault(t => t.LanguageId == 1);

                    if (enTranslation != null)
                    {
                        enTranslation.TranslatedKey = spec.Translations.FirstOrDefault(t => t.LanguageId == 2)?.TranslatedKey;
                        enTranslation.TranslatedValue = spec.Translations.FirstOrDefault(t => t.LanguageId == 2)?.TranslatedValue;
                    }

                    if (arTranslation != null)
                    {
                        arTranslation.TranslatedKey = spec.Translations.FirstOrDefault(t => t.LanguageId == 1)?.TranslatedKey;
                        arTranslation.TranslatedValue = spec.Translations.FirstOrDefault(t => t.LanguageId == 1)?.TranslatedValue;
                    }

                    // Save changes
                    _productSpecificationService.UpdateSpecificationAsync(existingSpec);
                }
            }



            //ModelState.AddModelError("", result.Msg);
            //}
            foreach (var spec in productDto.Specifications)
            {
                await _productSpecificationService.UpdateSpecificationAsync(spec);
                foreach (var specTrans in spec.Translations)
                {
                    _productSpecificationTransService.UpdateTranslationAsync(specTrans);
                }
            }


            if (result.IsSuccess)
            {
                return RedirectToAction("Index", new { id = productDto.Id });
            }
            // Return the entire resultView to the view, since it's the expected type
            return View(resultView);
        }





        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var product = await productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await productService.DeleteProductAsync(id);
            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            return BadRequest(result.Msg);
        }



        //public async Task<IActionResult> GetDeleted(int id)
        //{
        //    
        //}





        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productB = await productService.GetProductByIdAsync(id);
            var categories = await productCategoryService.GetCategoriesByProductIdAsync(id);
            ViewBag.ProductCategories = categories.Entity;

            return View(productB);
        }

        public async Task<IActionResult> Search(string searchString)
        {
            var products = await productService.SearchProductsByNameAsync(searchString);
            //ViewBag.SearchString = searchString;

            return View("Index", products);
        }
    }
}
