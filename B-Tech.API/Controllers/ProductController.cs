using ApplicationB.Services_B.Category;
using ApplicationB.Services_B.General;
using ApplicationB.Services_B.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelsB.Category_B;

namespace B_Tech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IProductCategoryService ProductcategoryService;
        private readonly ILanguageService languageService;

        public ProductController(IProductService _productService, IProductCategoryService _ProductCategoryService, ILanguageService _languageService)
        {
            productService = _productService;
           
            languageService = _languageService;
            ProductcategoryService = _ProductCategoryService;
        }

        private string GetUserLanguage()
        {
            var userLanguages = Request.Headers["Accept-Language"].ToString();
            if (!string.IsNullOrEmpty(userLanguages))
            {
                // Use the first preferred language from the list
                return userLanguages.Split(',').FirstOrDefault();
            }

            // Default to English if no language is found
            return "en";
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userLanguage = GetUserLanguage();
            languageService.SetCurrentLanguageCode(userLanguage);
            var products = await productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("GetPaginated")]
        public async Task<IActionResult> GetPaginated([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var userLanguage = GetUserLanguage();
            languageService.SetCurrentLanguageCode(userLanguage);

            var lang = languageService.GetCurrentLanguageCode();
            var products = await productService.GetAllPaginatedByLanguageAsync(pageNumber, pageSize, lang);

            return Ok(products);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var userLanguage = GetUserLanguage();
            languageService.SetCurrentLanguageCode(userLanguage);
            var productB = await productService.GetProductByIdAsync(id);

            return Ok(productB);
        }
        [HttpGet("Search")]
        public async Task<IActionResult> Search(string searchString)
        {
            var userLanguage = GetUserLanguage();
            languageService.SetCurrentLanguageCode(userLanguage);
            var products = await productService.SearchProductsByNameAsync(searchString);

            return Ok(products);
        }


        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductsCategory(int CtaegoryId)
        {
            if (CtaegoryId == 0)
            {
                return BadRequest("Invalid category ID.");
            }
            var userLanguage = GetUserLanguage();
            languageService.SetCurrentLanguageCode(userLanguage);
            var products = await ProductcategoryService.GetProductsByCategoryIdAsync(CtaegoryId);

            if (products == null || !products.Entity.Any())
            {
                return NotFound("No products found for this category.");
            }

            return Ok(products);
        }
    }
}
