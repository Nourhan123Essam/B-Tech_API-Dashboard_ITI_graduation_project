using ApplicationB.Contracts_B;
using DbContextB;
using DTOsB.Shared;
using InfrastructureB.General;
using Microsoft.EntityFrameworkCore;
using ModelsB.Product_B;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureB.Product
{
    public class ProductRepository : GenericRepositoryWithLogging<ProductB>, IProductRepository
    {


        public ProductRepository(BTechDbContext context) : base(context)
        {
            
        }


        public async Task<IQueryable<ProductB>> SearchByNameAsync(string name)
        {
            var products = await GetAllAsync();  
            return products.Where(p => p.Translations.Any(t => t.Name.ToLower().Contains(name.ToLower()) ||
                 p.Translations.Any(t => t.BrandName.ToLower().Contains(name.ToLower())))).Include(p=>p.Translations).Include(p => p.Images);
        }

        public override async Task<ProductB> GetByIdAsync(int id)
        {
            return await _context.Products.Where(p => !p.IsDeleted).Include(p => p.Translations).Include(p => p.Images)
                .Include(p => p.Specifications).ThenInclude(s => s.Translations)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public override async Task<IQueryable<ProductB>> GetAllAsync()
        {
            
            return await Task.FromResult(_context.Products.Where(p => !p.IsDeleted).Include(p => p.Translations)
                .Include(p => p.Images));

        }

        public async Task<IQueryable<ProductB>> GetFilteredProductsAsync(int languageId)
        {
            var products = _context.Products.Where(p => !p.IsDeleted).Include(p => p.Translations.Where(t => t.Language.Id == languageId)).Include(p => p.Images);
            return products;
        }
        public async Task<EntityPaginatedB<ProductB>> GetAllPaginatedAsync(int pageNumber, int count)
        {
           
            if (pageNumber < 1) pageNumber = 1;
            if (count < 1) count = 10; 

         
            var skip = (pageNumber - 1) * count;

            var query = _context.Products 
                                .Include(p => p.Images)       
                                .Include(p => p.Translations) 
                                .Include(p => p.Specifications)
                                .ThenInclude(s => s.Translations)
                                .OrderBy(p => p.Id)   
                                .Skip(skip)                 
                                .Take(count);               

          
            var totalCount = await _context.Products.CountAsync();

           
            var products = await query.ToListAsync();

            
            var result = new EntityPaginatedB<ProductB>
            {
                Data = products,        // Paginated items
                PageNumber = pageNumber,  // Current page number
                Count = count,            // Items per page
                CountAllItems = totalCount   // Total items in the database
            };

            return result;
        }


        ///// <summary>
        ///// Product
        ///// </summary>










        ///// <summary>
        ///// Image
        ///// </summary>


        //public async Task AddImagesAsync(int productId, IEnumerable<ProductImageB> images)
        //{
        //    var product = await GetByIdAsync(productId);
        //    if (product != null)
        //    {
        //        foreach (var image in images)
        //        {
        //            image.ProductId = productId; // Set the foreign key
        //            await _context.ProductImages.AddAsync(image);
        //        }
        //        await _context.SaveChangesAsync();
        //    }
        //}

        //public async Task RemoveImageAsync(int productId, int imageId)
        //{
        //    var image = await _context.ProductImages.FindAsync(imageId);
        //    if (image != null && image.ProductId == productId)
        //    {
        //        _context.ProductImages.Remove(image);
        //        await _context.SaveChangesAsync();
        //    }
        //}


        ///// <summary>
        ///// Translation
        ///// </summary>
        //public async Task AddTranslationsAsync(int productId, IEnumerable<ProductTranslationB> translations)
        //{
        //    var product = await GetByIdAsync(productId);
        //    if (product != null)
        //    {
        //        foreach (var translation in translations)
        //        {
        //            translation.ProductId = productId; // Set the foreign key
        //            await _context.ProductTranslations.AddAsync(translation);
        //        }
        //        await _context.SaveChangesAsync();
        //    }
        //}

        //public async Task RemoveTranslationAsync(int productId, int translationId)
        //{
        //    var translation = await _context.ProductTranslations.FindAsync(translationId);
        //    if (translation != null && translation.ProductId == productId)
        //    {
        //        _context.ProductTranslations.Remove(translation);
        //        await _context.SaveChangesAsync();
        //    }
        //}

        ///// <summary>
        ///// Specification
        ///// </summary>

        //public async Task AddSpecificationsAsync(int productId, IEnumerable<ProductSpecificationsB> specifications)
        //{
        //    var product = await GetByIdAsync(productId);
        //    if (product != null)
        //    {
        //        foreach (var specification in specifications)
        //        {
        //            specification.ProductId = productId; // Set the foreign key
        //            await _context.ProductSpecifications.AddAsync(specification);
        //        }
        //        await _context.SaveChangesAsync();
        //    }
        //}

        //public async Task RemoveSpecificationAsync(int productId, int specificationId)
        //{
        //    var specification = await _context.ProductSpecifications.FindAsync(specificationId);
        //    if (specification != null && specification.ProductId == productId)
        //    {
        //        _context.ProductSpecifications.Remove(specification);
        //        await _context.SaveChangesAsync();
        //    }
        //}

        //public IQueryable<ProductSpecificationsB> GetSpecificationsByProductId(int productId)
        //{
        //    return _context.ProductSpecifications
        //        .Where(s => s.ProductId == productId);
        //    //}
        //}

    }
}