using DTOsB.Order.OrderDTO;
using DTOsB.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationB.Services_B.Order
{
    public interface IOrderService
    {
        Task<ResultView<AddOrUpdateOrderBDTO>> CreateOrderAsync(AddOrUpdateOrderBDTO orderBDTO);
        Task<ResultView<AddOrUpdateOrderBDTO>> UpdateOrderAsync(AddOrUpdateOrderBDTO orderBDTO);
        Task<ResultView<SelectOrderBDTO>> DeleteOrderAsync(int id);
        Task<SelectOrderBDTO> GetOrderByIdAsync(int id);
        Task<IEnumerable<SelectOrderBDTO>> GetAllOrdersAsync();
    }
}
