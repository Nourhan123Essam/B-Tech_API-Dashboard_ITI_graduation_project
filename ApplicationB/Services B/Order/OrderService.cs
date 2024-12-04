using ApplicationB.Contracts_B.Order;
using ApplicationB.Services_B.User;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DTOsB.Order.OrderDTO;
using DTOsB.Order.OrderItemDTO;
using DTOsB.Shared;
using ModelsB.Order_B;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationB.Services_B.Order
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;
        private readonly IUserService userService;
        private readonly IOrderItemService orderItemService;
        private readonly IShippingService shippingService;

        public OrderService(IOrderRepository _orderRepository, IMapper _mapper, IUserService _userService,
                            IOrderItemService _orderItemService, IShippingService _shippingService)
        {
            orderRepository = _orderRepository;
            mapper = _mapper;
            userService = _userService;
            orderItemService = _orderItemService;
            shippingService = _shippingService;
        }
        public async Task<ResultView<AddOrUpdateOrderBDTO>> CreateOrderAsync(AddOrUpdateOrderBDTO orderBDTO)
        {
            var order = mapper.Map<OrderB>(orderBDTO);

            order.CreatedBy = userService.GetCurrentUserId();
            order.Created = DateTime.Now;
            order.UpdatedBy = userService.GetCurrentUserId();
            order.Updated = DateTime.Now;
            order.OrderDate = DateTime.Now;

            await orderRepository.AddAsync(order);
            return ResultView<AddOrUpdateOrderBDTO>.Success(orderBDTO);
        }

        //*************************************************************

        public async Task<ResultView<SelectOrderBDTO>> DeleteOrderAsync(int id)
        {
            var existingOrder = await orderRepository.GetByIdAsync(id);
            if (existingOrder == null)
                return ResultView<SelectOrderBDTO>.Failure("Shipping not found. Unable to delete.");

            existingOrder.IsDeleted = true;
            existingOrder.UpdatedBy = userService.GetCurrentUserId();
            existingOrder.Updated = DateTime.Now;

            await orderRepository.UpdateAsync(existingOrder);
            return ResultView<SelectOrderBDTO>.Success(null);
        }

        //*************************************************************

        public async Task<IEnumerable<SelectOrderBDTO>> GetAllOrdersAsync()
        {
            var orders = await orderRepository.GetAllAsync();
            //var res = new List<SelectOrderBDTO>();
            //foreach(var order in orders)
            //{
            //    var ele = new SelectOrderBDTO();
            //    ele = mapper.Map<SelectOrderBDTO>(order);
            //    res.Add(
            //}
            return orders.ProjectTo<SelectOrderBDTO>(mapper.ConfigurationProvider);
        }

        //*************************************************************

        public async Task<SelectOrderBDTO> GetOrderByIdAsync(int id)
        {
            OrderB order = await orderRepository.GetByIdAsync(id);
           

            var orderDto = mapper.Map<SelectOrderBDTO>(order);
            
            var test = new SelectOrderBDTO();
            if (orderDto == null) return test;

            var items = await orderItemService.GetAllItemsOfOrderAsync(orderDto.Id);
            if (items == null) items = new List<SelectOrderItemBDTO>();
            
            orderDto.OrderItems = items;

            orderDto.ApplicationUserName = (await userService.GetAppUserByIdAsync(order.ApplicationUserId)).UserName;
            orderDto.ApplicationUserId = order.ApplicationUserId;

            var shippingResulView = await shippingService.GetShippingByIdAsync(order.ShippingId);
            if (shippingResulView.Entity == null) orderDto.ShippingCost = 70;
            else orderDto.ShippingCost = shippingResulView.Entity.ShippingCost;

            if( order.Payment?.PaymentStatus==null) orderDto.PaymentStatus="Pending";

            return orderDto;
        }

        //*************************************************************

        public async Task<ResultView<AddOrUpdateOrderBDTO>> UpdateOrderAsync(AddOrUpdateOrderBDTO orderBDTO)
        {
            if (orderBDTO == null)
                return ResultView<AddOrUpdateOrderBDTO>.Failure("Order data cannot be null.");

            var existingOrder = await orderRepository.GetByIdAsync(orderBDTO.Id);
            if (existingOrder == null)
                return ResultView<AddOrUpdateOrderBDTO>.Failure("Order not found. Unable to update.");

            mapper.Map(orderBDTO, existingOrder);
            //existingOrder.UpdatedBy = userService.GetCurrentUserId();
            //existingOrder.Updated = DateTime.Now;

            await orderRepository.UpdateAsync(existingOrder);
            return ResultView<AddOrUpdateOrderBDTO>.Success(orderBDTO);
        }
    }
}
