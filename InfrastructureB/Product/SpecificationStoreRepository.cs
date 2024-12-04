using ApplicationB.Contracts_B;
using ApplicationB.Contracts_B.Product;
using DbContextB;
using InfrastructureB.General;
using ModelsB.Product_B;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureB.Product
{
    public class SpecificationStoreRepository : GenericRepositoryB<SpecificationStore>, ISpecificationStoreRepository
    {
        public SpecificationStoreRepository(BTechDbContext context):base(context)
        {
            
        }
    }
}
