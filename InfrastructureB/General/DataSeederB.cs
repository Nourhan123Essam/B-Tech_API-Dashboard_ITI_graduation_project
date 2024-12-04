using ModelsB.Category_B;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbContextB;
using Microsoft.Extensions.DependencyInjection;
using Bogus;
using ModelsB.Product_B;
using ModelsB.Authentication_and_Authorization_B;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureB.General
{
    public class DataSeederB
    {
        private readonly BTechDbContext _context;

        public DataSeederB(BTechDbContext context)
        {
            _context = context;
        }

        //public void Seed()
        //{

        //    _context.Database.EnsureCreated();

        //var categoryFaker = new Faker<ApplicationUserB>()
        //    .RuleFor(c => c.Email, f => f.);

        //var categoryFaker = new Faker<CategoryB>()
        //    .RuleFor(c => c.ImageUrl, f => f.Commerce.Categories(1)[0]);


        //var categories = categoryFaker.Generate(10);  // Generate 10 fake categories

        //// Add categories to the context
        //_context.Categories.AddRange(categories);
        //_context.SaveChanges();

        //// Similarly, you can generate fake data for other entities like ProductB, OrderB, etc.
        //var productFaker = new Faker<ProductB>()
        //.RuleFor(p => p., f => f.Commerce.ProductName())
        //.RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price()))
        //.RuleFor(p => p.Description, f => f.Commerce.ProductDescription());

        //var products = productFaker.Generate(20);  // Generate 20 fake products
        //_context.Products.AddRange(products);
        //    _context.SaveChanges();
        //}
        public async Task SeedAsync()
        {
            // Step 1: Retrieve existing Categories with Translations from the database
            var categories = await SeedCategoriesAsync();

            // Step 2: Seed Products with related data using existing categories
            var products = await SeedProducts(categories);

            foreach (var product in products)
            {
                try
                {
                    // Check if the product already exists
                    if (!_context.Products.Any(p => p.Id == product.Id))
                    {
                        product.CreatedBy = "38981d39-6f02-4c16-a231-292adf2e5637";
                        product.UpdatedBy = "38981d39-6f02-4c16-a231-292adf2e5637";

                        // Check for existing translations before adding the product
                        if (!_context.ProductSpecificationTranslations.Any(t =>
                            t.SpecificationId == product.Id && t.LanguageId == 2)) // assuming LanguageId 2 is the duplicate
                        {
                            _context.Products.Add(product);
                        }
                        else
                        {
                            Console.WriteLine($"Product {product.Id} already has a translation for LanguageId: 2.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Product {product.Id} already exists.");
                    }
                }
                catch (DbUpdateException ex)
                {
                    // Handle duplicate key exception
                    if (ex.InnerException != null &&
                        ex.InnerException.Message.Contains("duplicate key row"))
                    {
                        Console.WriteLine($"Error: Duplicate product specification translation for SpecificationId: {product.Id}, LanguageId: 2");
                    }
                    else
                    {
                        // Log other types of exceptions for further analysis
                        Console.WriteLine($"Error updating product {product.Id}: {ex.Message}");
                        throw; // Rethrow if you can't handle the exception
                    }
                }
            }

            await _context.SaveChangesAsync();
        }


        private async Task<List<CategoryB>> SeedCategoriesAsync()
        {
            // Load categories from the database along with their translations
            var categories = await _context.Categories
                .Include(c => c.Translations)
                .ToListAsync();

            return categories;
        }

        private async Task<List<ProductB>> SeedProducts(List<CategoryB> categories)
        {
            var productFaker = new Faker<ProductB>()
                .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price())) // Parse to decimal
                .RuleFor(p => p.StockQuantity, f => f.Random.Int(1, 100))
                .RuleFor(p => p.ProductCategories, f =>
                {
                    var productCategories = new List<ProductCategoryB>();

                    // Associate each product with 1-3 random categories
                    var assignedCategories = f.PickRandom(categories, f.Random.Int(1, 2));
                    foreach (var category in assignedCategories)
                    {
                        productCategories.Add(new ProductCategoryB
                        {
                            Category = category,
                            IsMainCategory = f.Random.Bool()
                        });
                    }
                    return productCategories;
                })
                .RuleFor(p => p.Images, f => SeedImages())
                .RuleFor(p => p.Specifications, f => SeedSpecifications())
                .RuleFor(p => p.Translations, f => SeedProductTranslations(f)); // Pass the Faker instance

            var products = new List<ProductB>();

            while (products.Count < 100) // Generate 100 unique products
            {
                var newProduct = productFaker.Generate();

                // Check for duplicates in the database
                var existingProduct = await _context.Products
                    .AnyAsync(p => p.Translations.FirstOrDefault().Name == newProduct.Translations.FirstOrDefault().Name); // Change this to whatever unique identifier you use

                if (!existingProduct)
                {
                    products.Add(newProduct);
                }
                else
                {
                    // Optionally log that a duplicate was found
                    Console.WriteLine($"Duplicate product found: {newProduct.Translations.FirstOrDefault().Name}. Generating a new one.");
                }
            }

            return products;
        }



        private List<ProductImageB> SeedImages()
        {
            var imageFaker = new Faker<ProductImageB>()
                .RuleFor(img => img.Url, f => f.Image.PicsumUrl());

            return imageFaker.Generate(4); // Each product gets 3 images
        }

        private List<ProductSpecificationsB> SeedSpecifications()
        {
            var specFaker = new Faker<ProductSpecificationsB>()
                .RuleFor(spec => spec.Translations, f => SeedSpecTranslations());

            return specFaker.Generate(10); // Each product gets 5 specifications
        }

        private List<ProductSpecificationTranslationB> SeedSpecTranslations()
        {
            var translationFaker = new Faker<ProductSpecificationTranslationB>()
                .RuleFor(tr => tr.TranslatedKey, f => f.Commerce.ProductMaterial())
                .RuleFor(tr => tr.TranslatedValue, f => f.Commerce.ProductName())
                .RuleFor(tr => tr.LanguageId, f => f.PickRandom(1, 2));

            return translationFaker.Generate(2); // Each specification gets translations in both languages
        }

        private List<ProductTranslationB> SeedProductTranslations(Faker faker)
        {
            return new List<ProductTranslationB>
    {
        new ProductTranslationB
        {
            LanguageId = 1, // Arabic
            Name = faker.Commerce.ProductName(), // Generate a random product name
            BrandName = faker.Company.CompanyName(), // Generate a random brand name
            Description = faker.Commerce.ProductDescription() // Generate a random product description
        },
        new ProductTranslationB
        {
            LanguageId = 2, // English
            Name = faker.Commerce.ProductName(), // Generate a random product name
            BrandName = faker.Company.CompanyName(), // Generate a random brand name
            Description = faker.Commerce.ProductDescription() // Generate a random product description
        }
    };
        }


    }
}
