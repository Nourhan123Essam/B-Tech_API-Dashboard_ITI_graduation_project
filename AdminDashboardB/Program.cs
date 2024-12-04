using ApplicationB.Contracts_B.Product;
using ApplicationB.Contracts_B;
using ApplicationB.Mapper_B;
using ApplicationB.Services_B.Product;
using DbContextB;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ModelsB.Authentication_and_Authorization_B;
using System.Globalization;
using WebApplication1.Data;
using InfrastructureB.Product;
using ApplicationB.Services_B;
using ApplicationB.Contracts_B.General;
using ApplicationB.Services_B.General;
using InfrastructureB.General;
using ApplicationB.Contracts_B.Category;
using ApplicationB.Services_B.Category;
using InfrastructureB.Category;
using AutoMapper;
using AdminDashboardB.Models;
using ApplicationB.Contracts_B.Order;
using ApplicationB.Services_B.Order;
using InfrastructureB.Order;
using ApplicationB.Services_B.User;
using ApplicationB.Contracts_B.User;
using InfrastructureB.User;

namespace WebApplication1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            
            builder.Services.AddDbContext<BTechDbContext>(options =>
           options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<ApplicationUserB, IdentityRole>().AddEntityFrameworkStores<BTechDbContext>()
                   .AddDefaultTokenProviders().AddDefaultUI();


          
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();


            //builder.Services.AddControllersWithViews(options =>
            //{
            //    options.Filters.Add(new AuthorizeFilter());
            //});
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Admin/Login"; // Redirect to Admin login page
                options.AccessDeniedPath = "/Home/AccessDenied"; // Optional: redirect to an access denied page
                options.SlidingExpiration = true; // Renew the session on each request
                options.ExpireTimeSpan = TimeSpan.FromDays(7); // Adjust duration as needed
                options.Cookie.HttpOnly = true; // Helps protect against XSS attacks
                options.Cookie.IsEssential = true;
            });


            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
            builder.Services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            builder.Services.AddAutoMapper(typeof(MappingProfile));
            var serviceProvider = builder.Services.BuildServiceProvider();
            var mapper = serviceProvider.GetService<IMapper>();
            //mapper.ConfigurationProvider.AssertConfigurationIsValid();

            builder.Services.AddHttpContextAccessor();


            #region AddScoped

            builder.Services.AddScoped<ILanguageRepository, LanguageRepository>();
            builder.Services.AddScoped<ILanguageService, LanguageService>();
            builder.Services.AddScoped<IImageService, ImageService>();

            

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();

            //==========Product==========

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService,ProductService>();
            builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
            builder.Services.AddScoped<IProductImageService,ProductImageService>();

            builder.Services.AddScoped<IProductSpecificationRepository, ProductSpecificationRepository>();
            builder.Services.AddScoped<IProductSpecificationService,ProductSpecificationService>();
            builder.Services.AddScoped<IProductTranslationRepository, ProductTranslationRepository>();
            builder.Services.AddScoped<IProductTranslationService,ProductTranslationService>();
            builder.Services.AddScoped<IProductSpecificationTranslationRepository, ProductSpecificationTranslationRep>();
            builder.Services.AddScoped<IProductSpecificationTransService, ProductSpecificationTransService>();
            builder.Services.AddScoped<ISpecificationStoreRepository, SpecificationStoreRepository>();
            builder.Services.AddScoped<ISpecificationStoreService, SpecificationStoreService>();


            builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
            builder.Services.AddScoped<IReviewService, ReviewService>();


            //==========Category==========

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();


            //===========Order==============
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderService, OrderService>();

            builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            builder.Services.AddScoped<IOrderItemService, OrderItemService>();

            builder.Services.AddScoped<IShippingRepository, ShippingRepository>();
            builder.Services.AddScoped<IShippingService, ShippingService>();

            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddScoped<PaymentService>();
            #endregion



            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("ar-EG")
                };

                options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            //builder.Services.AddTransient<DataSeederB>();




            builder.Services.AddControllersWithViews();
           
            var app = builder.Build();


            //using (var scope = app.Services.CreateScope())
            //{
            //    var seeder = scope.ServiceProvider.GetRequiredService<DataSeederB>();
            //    await seeder.SeedAsync(); // Call the seed method asynchronously
            //}

          
            //using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
            //{
            //    var services = serviceScope.ServiceProvider;
            //    try
            //    {
            //        RoleInitializer.Initialize(services).Wait();
            //    }
            //    catch (Exception ex)
            //    {
            //        // Log the error or handle accordingly
            //    }
            //}

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            var locOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            await app.RunAsync();
        }
    }
}
