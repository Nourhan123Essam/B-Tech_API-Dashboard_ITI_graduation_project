using DTOsB.Order.OrderItemDTO;
using DTOsB.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationB.Services_B.Order
{
    public interface IOrderItemService
    {
        Task<ResultView<AddOrUpdateOrderItemBDTO>> CreateOrderItemAsync(AddOrUpdateOrderItemBDTO orderItemBDTO);
        Task<ResultView<AddOrUpdateOrderItemBDTO>> UpdateOrderItemAsync(AddOrUpdateOrderItemBDTO orderItemBDTO);
        Task<ResultView<SelectOrderItemBDTO>> DeleteOrderItemAsync(int id);
        Task<ResultView<SelectOrderItemBDTO>> GetOrderItemByIdAsync(int id);
        Task<IEnumerable<SelectOrderItemBDTO>> GetAllOrderItemsAsync();
        Task<IEnumerable<SelectOrderItemBDTO>> GetAllItemsOfOrderAsync(int id);
    }
}
