using ApplicationB.Contracts_B.Product;
using DbContextB;
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
    public class ProductSpecificationRepository : GenericRepositoryB<ProductSpecificationsB>, IProductSpecificationRepository
    {
        public ProductSpecificationRepository(BTechDbContext context) : base(context) { }

        public async Task<IQueryable<ProductSpecificationsB>> GetSpecificationsByProductId(int productId)
        {
            var specifications = await GetAllAsync();
            return specifications.Include(spec => spec.Translations)
                .Where(specs => specs.ProductId == productId && specs.Product.IsDeleted == false);
        }

    }
}
