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
    public  class ProductTranslationRepository : GenericRepositoryB<ProductTranslationB>, IProductTranslationRepository
    {
        public ProductTranslationRepository(BTechDbContext context) : base(context) { }

        public async Task<IQueryable<ProductTranslationB>> GetTranslationsByProductId(int productId, int languageId)
        {
            var translations = await GetAllAsync();
            return translations.Where(trans => trans.ProductId == productId && trans.Product.IsDeleted==false 
            && trans.Language.Id== languageId);
        }
    }
}
