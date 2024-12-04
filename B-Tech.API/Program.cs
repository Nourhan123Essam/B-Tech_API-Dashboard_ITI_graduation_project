
using ApplicationB.Contracts_B;
using ApplicationB.Contracts_B.Category;
using ApplicationB.Contracts_B.General;
using ApplicationB.Contracts_B.Order;
using ApplicationB.Contracts_B.Product;
using ApplicationB.Contracts_B.User;
using ApplicationB.Mapper_B;
using ApplicationB.Services_B;
using ApplicationB.Services_B.Category;
using ApplicationB.Services_B.General;
using ApplicationB.Services_B.Order;
using ApplicationB.Services_B.Product;
using ApplicationB.Services_B.User;
using AutoMapper;
using DbContextB;
using InfrastructureB.Category;
using InfrastructureB.General;
using InfrastructureB.Order;
using InfrastructureB.Product;
using InfrastructureB.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ModelsB.Authentication_and_Authorization_B;
using System.Globalization;
using System.Text;

namespace B_Tech.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<PayPalService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost", builder =>
                {
                    builder.AllowAnyOrigin()//.WithOrigins("http://127.0.0.1:5500") // Allow your frontend origin
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            // Add services to the container.
            builder.Services.AddDbContext<BTechDbContext>(options =>
         options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(op =>
            {
              op.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateIssuer = true,
                  ValidateAudience = true,
                  ValidateLifetime = true,
                  ValidateIssuerSigningKey = true,
                  ValidIssuer = builder.Configuration["jwt:Issuer"],
                  ValidAudience = builder.Configuration["jwt:Audience"],
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:Key"]))
              };
          });


            builder.Services.AddIdentity<ApplicationUserB, IdentityRole>().AddEntityFrameworkStores<BTechDbContext>()
                  .AddRoles<IdentityRole>();


            //********* paypal ***********
            builder.Services.AddHttpClient<PayPalService>();


            //builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Admin/Login"; // Redirect to Admin login page
                options.AccessDeniedPath = "/Home/AccessDenied"; // Optional: redirect to an access denied page
                options.SlidingExpiration = true; // Renew the session on each request
                options.ExpireTimeSpan = TimeSpan.FromDays(7); // Adjust duration as needed
                options.Cookie.HttpOnly = true; // Helps protect against XSS attacks
                options.Cookie.IsEssential = true;
            });


            //.AddRoles<ApplicationUserB>();


            builder.Services.AddCors(op =>
            {
                op.AddPolicy("Default", policy =>
                {
                    policy.AllowAnyHeader()
                          .AllowAnyOrigin()
                          .AllowAnyMethod();
                });
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c => {
                c.SwaggerDoc(
                    "v1", new OpenApiInfo
                    {
                        Title = "JWTToken_Auth_API",
                        Version = "v1"
                    });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
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
            //builder.Services.AddScoped<IImageService, ImageService>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();


            //==========Product==========

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
            builder.Services.AddScoped<IProductImageService, ProductImageService>();

            builder.Services.AddScoped<IProductSpecificationRepository, ProductSpecificationRepository>();
            builder.Services.AddScoped<IProductSpecificationService, ProductSpecificationService>();
            builder.Services.AddScoped<IProductTranslationRepository, ProductTranslationRepository>();
            builder.Services.AddScoped<IProductTranslationService, ProductTranslationService>();
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
            builder.Services.AddScoped<IPaymentService, PaymentService>();


            //==========Language==========
            builder.Services.AddScoped<ILanguageRepository, LanguageRepository>();
            builder.Services.AddScoped<ILanguageService, LanguageService>();
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


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
      
           



            var app = builder.Build();

            //allow for the angular connection
            app.UseCors("AllowLocalhost");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "B-Tech API v1");
            });

            app.UseHttpsRedirection();
            app.UseCors("Default");


            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
