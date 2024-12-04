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
    public class ReviewRepository : GenericRepositoryB<ReviewB>, IReviewRepository
    {
        public ReviewRepository(BTechDbContext context):base(context) { }
        
    }
}
