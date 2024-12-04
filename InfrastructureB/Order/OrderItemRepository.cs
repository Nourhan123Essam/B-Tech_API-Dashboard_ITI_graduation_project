using ApplicationB.Contracts_B.Order;
using DbContextB;
using InfrastructureB.General;
using Microsoft.EntityFrameworkCore;
using ModelsB.Order_B;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureB.Order
{
    public class OrderItemRepository : GenericRepositoryB<OrderItemB>, IOrderItemRepository
    {
        private readonly BTechDbContext context;

        public OrderItemRepository(BTechDbContext _context) : base(_context)
        {
            context = _context;
        }

        public async Task<IEnumerable<OrderItemB>> ItemsOfOrder(int id)
        {
            var ans = context.OrderItems.Where(o => o.OrderId == id && o.IsDeleted == false).Include(p => p.Product)
                .ThenInclude(t => t.Translations)
                .Include(o => o.Product)              
                .ThenInclude(p => p.Images);
            foreach(var item in ans)
            {
                var idtest = item.Id;
            }
            return ans;
        }
    }
}
