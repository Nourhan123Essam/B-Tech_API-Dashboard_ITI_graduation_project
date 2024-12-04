using ApplicationB.Contracts_B.Order;
using ApplicationB.Services_B.User;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DTOsB.Order.ShippingDTO;
using DTOsB.Shared;
using ModelsB.Order_B;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationB.Services_B.Order
{
    public class ShippingService : IShippingService
    {
        private readonly IShippingRepository shippingRepository;
        private readonly IMapper mapper;
        private readonly IUserService userService;

        public ShippingService(IShippingRepository _shippingRepository, IMapper _mapper, IUserService _userService)
        {
            shippingRepository = _shippingRepository;
            mapper = _mapper;
            userService = _userService;

        }
        public async Task<ResultView<AddOrUpdateShippingBDTO>> CreateShippingAsync(AddOrUpdateShippingBDTO shippingBDTO)
        {
            var shipping = mapper.Map<ShippingB>(shippingBDTO);

            //shipping.CreatedBy = userService.GetCurrentUserId();
            //shipping.Created = DateTime.Now;

            await shippingRepository.AddAsync(shipping);
            return ResultView<AddOrUpdateShippingBDTO>.Success(shippingBDTO);
        }

        public async Task<ResultView<SelectShippingBDTO>> DeleteShippingAsync(int id)
        {
            var existingShipping = await shippingRepository.GetByIdAsync(id);
            if (existingShipping == null)
                return ResultView<SelectShippingBDTO>.Failure("Shipping not found. Unable to delete.");

            //existingShipping.IsDeleted = true;
            //existingShipping.UpdatedBy = userService.GetCurrentUserId();
            //existingShipping.Updated = DateTime.Now;

            await shippingRepository.UpdateAsync(existingShipping);
            return ResultView<SelectShippingBDTO>.Success(null);
        }

        public async Task<IEnumerable<SelectShippingBDTO>> GetAllShippingsAsync()
        {
            var shippings = await shippingRepository.GetAllAsync();
            return shippings.ProjectTo<SelectShippingBDTO>(mapper.ConfigurationProvider);
        }

        public async Task<ResultView<SelectShippingBDTO>> GetShippingByIdAsync(int id)
        {
            var shipping = await shippingRepository.GetByIdAsync(id);
            if (shipping == null)
                return ResultView<SelectShippingBDTO>.Failure("Shipping not found.");

            var shippingDto = mapper.Map<SelectShippingBDTO>(shipping);
            return ResultView<SelectShippingBDTO>.Success(shippingDto);
        }

        public async Task<ResultView<AddOrUpdateShippingBDTO>> UpdateShippingAsync(AddOrUpdateShippingBDTO shippingBDTO)
        {

            if (shippingBDTO == null)
                return ResultView<AddOrUpdateShippingBDTO>.Failure("Shipping data cannot be null.");

            var existingShipping = await shippingRepository.GetByIdAsync(shippingBDTO.Id);
            if (existingShipping == null)
                return ResultView<AddOrUpdateShippingBDTO>.Failure("Shipping not found. Unable to update.");

            if (shippingBDTO.ShippingCost < 0)
                return ResultView<AddOrUpdateShippingBDTO>.Failure("Shipping Cost must be a positive value.");

            //mapper.Map(shippingBDTO, existingShipping);
            //existingShipping.UpdatedBy = userService.GetCurrentUserId();
            //existingShipping.Updated = DateTime.Now;

            await shippingRepository.UpdateAsync(existingShipping);
            return ResultView<AddOrUpdateShippingBDTO>.Success(shippingBDTO);
        }
    }
}
