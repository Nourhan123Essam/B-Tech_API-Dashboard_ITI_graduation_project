using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ModelsB.Authentication_and_Authorization_B;
using ModelsB.Category_B;
using ModelsB.Localization_B;
using ModelsB.Order_B;
using ModelsB.Product_B;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DbContextB
{
    public class BTechDbContext : IdentityDbContext<ApplicationUserB>
    {
      


        public BTechDbContext(DbContextOptions<BTechDbContext> options)
        : base(options)
        {
           
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUserB>().ToTable("Users", "security");
            builder.Entity<IdentityRole>().ToTable("Roles", "security");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "security");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "security");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "security");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "security");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "security");


            builder.Entity<ProductB>().HasMany(c => c.Translations).WithOne(ct => ct.Product).HasForeignKey(ct => ct.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ProductTranslationB>().HasIndex(pt => new { pt.ProductId, pt.LanguageId }).IsUnique();
            builder.Entity<ProductSpecificationsB>().HasMany(c => c.Translations).WithOne(ct => ct.ProductSpecification)
                .HasForeignKey(ct => ct.SpecificationId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ProductSpecificationTranslationB>().HasIndex(pst => new { pst.SpecificationId, pst.LanguageId })
                .IsUnique();

            builder.Entity<CategoryB>().HasMany(c => c.Translations).WithOne(ct => ct.Category).HasForeignKey(ct => ct.CategoryId)
                .OnDelete(DeleteBehavior.Cascade); 
            builder.Entity<CategoryTranslationB>().HasIndex(ct => new { ct.CategoryId, ct.LanguageId }).IsUnique();

            builder.Entity<ProductCategoryB>().HasKey(pc => new { pc.ProductId, pc.CategoryId });  
            builder.Entity<ProductCategoryB>().HasOne(pc => pc.Product).WithMany(p => p.ProductCategories)
                .HasForeignKey(pc => pc.ProductId);
            builder.Entity<ProductCategoryB>().HasOne(pc => pc.Category).WithMany(c => c.ProductCategories)
                .HasForeignKey(pc => pc.CategoryId);

            builder.Entity<OrderB>().HasOne(o => o.Payment).WithOne(p => p.Order).HasForeignKey<PaymentB>(p => p.OrderId);
            builder.Entity<OrderB>().HasOne(o => o.Shipping).WithOne(p => p.Order).HasForeignKey<ShippingB>(p => p.OrderId);
            builder.Entity<OrderB>().HasOne(o => o.ApplicationUser).WithMany().HasForeignKey(o => o.ApplicationUserId);

            builder.Entity<DiscountB>().HasIndex(d => d.Code).IsUnique();

            builder.Entity<ReviewB>().HasIndex(r => new { r.ProductId, r.ApplicationUserId }).IsUnique();
            builder.Entity<ReviewB>().HasOne(r => r.ApplicationUser).WithMany().HasForeignKey(r => r.ApplicationUserId);

            var hasher = new PasswordHasher<ApplicationUserB>();
            var adminUser = new ApplicationUserB
            {
                UserName = "Mohammed Abbas",
                Email = "moh.alnoby216@gmail.com",
                Address = "Hamza St",
                City = "Sohag",
                PostalCode = "12345",
                Country = "Egypt",
                UserType = "Admin"
            };
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Btech@g5");

            builder.Entity<ApplicationUserB>().HasData(adminUser);
        }

        /// <summary>
        /// Product DbSets
        /// </summary>
        public virtual DbSet<ProductB> Products { get; set; }
        public virtual DbSet<ProductImageB> ProductImages { get; set; }
        public virtual DbSet<ProductTranslationB> ProductTranslations { get; set; }
        public virtual DbSet<ProductSpecificationsB> ProductSpecifications { get; set; }
        public virtual DbSet<ProductSpecificationTranslationB> ProductSpecificationTranslations { get; set; }
        public virtual DbSet<SpecificationStore> SpecificationKeys { get; set; }
        public virtual DbSet<ReviewB> Reviews { get; set; }

        /// <summary>
        /// Category DbSets
        /// </summary>

        public virtual DbSet<CategoryB> Categories { get; set; }
        public virtual DbSet<CategoryTranslationB> CategoryTranslations { get; set; }
        public virtual DbSet<ProductCategoryB> ProductCategories { get; set; }
        

        /// <summary>
        /// Order DbSets
        /// </summary>

        public virtual DbSet<OrderB> Orders { get; set; }
        public virtual DbSet<OrderItemB> OrderItems { get; set; }
        public virtual DbSet<ShippingB> Shippings { get; set; }
        public virtual DbSet<PaymentB> Payments { get; set; }
        public virtual DbSet<DiscountB> Discounts { get; set; }


        /// <summary>
        /// Other DbSets
        /// </summary>
        //public virtual DbSet<SellerB> Sellers { get; set; }
        public virtual DbSet<LanguageB> Languages { get; set; }
        public virtual DbSet<LocalizationResourceB> LocalizationResources { get; set; }


       
      
       




        //public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    var userId = GetCurrentUserId(); // Method to get the current user's ID

        //    foreach (var entry in ChangeTracker.Entries<BaseEntityB>())
        //    {
        //        if (entry.State == EntityState.Added)
        //        {
        //            entry.Entity.CreatedBy = userId;
        //            entry.Entity.Created = DateTime.Now;
        //            entry.Entity.IsDeleted = false; 
        //        }

        //        if (entry.State == EntityState.Modified)
        //        {
        //            entry.Entity.UpdatedBy = userId;
        //            entry.Entity.Updated = DateTime.Now;
        //        }
        //    }
        //    return await base.SaveChangesAsync(cancellationToken);
        //}

        //private int GetCurrentUserId()
        //{
        //    var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    return userId != null ? int.Parse(userId) : 0;
        //}
    }
}
