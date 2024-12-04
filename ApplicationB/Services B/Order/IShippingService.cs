using DTOsB.Order.ShippingDTO;
using DTOsB.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationB.Services_B.Order
{
    public interface IShippingService
    {
        Task<ResultView<AddOrUpdateShippingBDTO>> CreateShippingAsync(AddOrUpdateShippingBDTO shippingBDTO);
        Task<ResultView<AddOrUpdateShippingBDTO>> UpdateShippingAsync(AddOrUpdateShippingBDTO shippingBDTO);
        Task<ResultView<SelectShippingBDTO>> DeleteShippingAsync(int id);
        Task<ResultView<SelectShippingBDTO>> GetShippingByIdAsync(int id);
        Task<IEnumerable<SelectShippingBDTO>> GetAllShippingsAsync();
    }
}
