using ModelsB.Product_B;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationB.Contracts_B.Product
{
    public interface IProductSpecificationRepository : IGenericRepositoryB<ProductSpecificationsB>
    {
        Task<IQueryable<ProductSpecificationsB>> GetSpecificationsByProductId(int productId);
    }
}
