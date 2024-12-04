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
    public class ProductImageRepository : GenericRepositoryB<ProductImageB>, IProductImageRepository
    {
        public ProductImageRepository(BTechDbContext context) : base(context) { }

        public IQueryable<ProductImageB> GetImagesByProductId(int productId)
        {
            return _context.ProductImages
             .Where(pi => pi.ProductId == productId);
        }

       
    }
}
