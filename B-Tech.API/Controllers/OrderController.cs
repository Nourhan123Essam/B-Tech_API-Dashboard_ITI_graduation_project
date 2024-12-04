using ApplicationB.Services_B.Order;
using ApplicationB.Services_B.Product;
using AutoMapper;
using DTOsB.Order.OrderDTO;
using DTOsB.Order.OrderItemDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ModelsB.Authentication_and_Authorization_B;
using ModelsB.Order_B;
using PayPalCheckoutSdk.Orders;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace B_Tech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous] // Allows access without authentication
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly IProductService productService;
        private readonly IProductTranslationService productTranslationService;
        private readonly IOrderItemService orderItemService;
        private IMapper mapper;

        public OrderController(IOrderService _orderService, IProductService _productService,
            IProductTranslationService _productTranslationService, IOrderItemService _orderItemService,
            IMapper _mapper)
        {
            orderService = _orderService;
            productService = _productService;
            productTranslationService = _productTranslationService;
            orderItemService = _orderItemService;
            mapper = _mapper;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        //**********************************************************
        [HttpPut("finish-order")]
        public async Task<IActionResult> finshOrder(int orderId, decimal total, string user)
        {
            var order = (await orderService.GetOrderByIdAsync(orderId));

            if (order == null || order.IsDeleted == true)
            {
                return NotFound("Order item not found.");
            }

            var orderUp = mapper.Map<AddOrUpdateOrderBDTO>(order);
            orderUp.CurrentStatus = Status.Pending;
            orderUp.TotalPrice = total;
            await orderService.UpdateOrderAsync(orderUp);

            foreach(var item in orderUp.orderItems)
            {
                var prduct = (await productService.GetProductByIdAsync(item.ProductId)).Entity;
                prduct.StockQuantity -= item.Quantity;
                await productService.UpdateProductAsync(prduct);
            }

            return Ok(new { message = "Order updated successfully." });
        }

        //**********************************************************

        [HttpPut("update-order-item-quantity")]
        public async Task<IActionResult> UpdateOrderItemQuantity(int orderItemId, int newQuantity)
        {
            var orderItem = (await orderItemService.GetOrderItemByIdAsync(orderItemId)).Entity;

            if (orderItem == null || orderItem.IsDeleted == true)
            {
                return NotFound("Order item not found.");
            }

            // Check if there’s enough stock
            var product = (await productService.GetProductByIdAsync(orderItem.ProductId)).Entity;

            if (product.StockQuantity < newQuantity)
            {
                return BadRequest("Not enough stock available.");
            }

            // Update quantity
            AddOrUpdateOrderItemBDTO updatedOrderItem = mapper.Map<AddOrUpdateOrderItemBDTO>(orderItem);
            updatedOrderItem.Quantity = newQuantity;
            await orderItemService.UpdateOrderItemAsync(updatedOrderItem);

            return Ok(new { message = "Order item quantity updated successfully." });
        }


        //**********************************************************

        [HttpPut("cancel-order")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var order = await orderService.GetOrderByIdAsync(orderId);

            if (order == null || order.IsDeleted == true)
            {
                return NotFound("Order not found.");
            }

            var updateOrder = mapper.Map<AddOrUpdateOrderBDTO>(order);
            updateOrder.CurrentStatus = Status.Cancelled;
            await orderService.UpdateOrderAsync(updateOrder);

            return Ok(new { message = "Order and all related items have been canceled." });
        }

        //*******************************************************

        [HttpDelete("order-item/{orderItemId}")]
        public async Task<IActionResult> DeleteOrderItem(int orderItemId)
        {
            var orderItem = (await orderItemService.GetOrderItemByIdAsync(orderItemId)).Entity;

            if (orderItem == null || orderItem.IsDeleted == true)
            {
                return NotFound(new { message = "Order item not found." });
            }

            await orderItemService.DeleteOrderItemAsync(orderItemId);

            return Ok(new { message = "Order item deleted successfully." });
        }



        //***************************************************************

        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCart(int productId, string userId)
        {
            // Find an existing "InCart" order for the user
            var order = (await orderService.GetAllOrdersAsync()).FirstOrDefault(p => p.CurrentStatus == ModelsB.Order_B.Status.InCart && p.ApplicationUserId == userId);
            var product = (await productService.GetProductByIdAsync(productId)).Entity;
            var productTranslation = (await productTranslationService.GetTranslationsByProductIdAsync(productId)).Entity?.FirstOrDefault();
            AddOrUpdateOrderBDTO addOrder;
            if (order == null)
            {
                addOrder = new AddOrUpdateOrderBDTO()
                {
                    CurrentStatus = ModelsB.Order_B.Status.InCart,
                    TotalPrice = product.Price,
                    OrderDate = DateTime.Now,
                    ApplicationUserId = userId,
                    CreatedBy = userId,
                    UpdatedBy = userId,
                };
                await orderService.CreateOrderAsync(addOrder);
                order = (await orderService.GetAllOrdersAsync()).FirstOrDefault(p => p.CurrentStatus == ModelsB.Order_B.Status.InCart && p.ApplicationUserId == userId);
            }
            else
            {
                addOrder = new AddOrUpdateOrderBDTO()
                {
                    Id = order.Id,
                    CurrentStatus = ModelsB.Order_B.Status.InCart,
                    TotalPrice = product.Price + order.TotalPrice,
                    OrderDate = order.OrderDate,
                    ApplicationUserId = userId,
                    UpdatedBy = userId,
                    CreatedBy = userId,
                    //  orderItems = (List<AddOrUpdateOrderItemBDTO>)order.OrderItems,
                };
                await orderService.UpdateOrderAsync(addOrder);
            }
            order = await orderService.GetOrderByIdAsync(order.Id);

            // Check if the product is already in the cart

            string pr = "Product One";
            SelectOrderItemBDTO orderItem;
            if (order == null) orderItem = new SelectOrderItemBDTO();
            else orderItem = order.OrderItems
                .FirstOrDefault(oi => oi.ProductId == productId && oi.OrderId == order.Id);

            if (orderItem != null)
            {
                // Update the quantity if the product is already in the cart
                var newOrderItem = mapper.Map<AddOrUpdateOrderItemBDTO>(orderItem);
                await orderItemService.UpdateOrderItemAsync(newOrderItem);
            }
            else
            {
                // Add a new OrderItem for the product
                var newOrderItem = new AddOrUpdateOrderItemBDTO()
                {
                    ProductId = productId,
                    Quantity = 1,
                    OrderId = order.Id,
                    CreatedBy = userId,
                    UpdatedBy = userId,
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    IsDeleted = false
                };

                await orderItemService.CreateOrderItemAsync(newOrderItem);

            }

            return Ok(new { message = "Product added successfully" });
        }

        //****************************************************************************

        [HttpGet]
        public async Task<IActionResult> ViewCart(string userId)

        {
            // Retrieve the "InCart" order for the user
            var order = (await orderService.GetAllOrdersAsync()).FirstOrDefault(p => p.CurrentStatus == ModelsB.Order_B.Status.InCart && p.ApplicationUserId == userId);

            if (order == null || order.IsDeleted == true)
            {
                return NotFound("You have no products in the cart.");
            }
            var inCartOrder = await orderService.GetOrderByIdAsync(order.Id);

            // Map OrderItems to a response DTO if needed
            var cartItems = inCartOrder.OrderItems.Select(oi => new
            {
                Id = oi.Id,
                Quantity = oi.Quantity,
                ProductName = oi.ProductName,
                ProductPrice = oi.Price,
                TotalPrice = oi.TotalPrice,
                StockQuantity = oi.StockQuantity,
                imageUrl = oi.Url,
                orderId = order.Id
            }).ToList();

            return Ok(cartItems);
        }




        //////////////////////////
        [HttpGet("user-orders")]
        public async Task<IActionResult> userOrders(string userId)

        {
            // Retrieve the "InCart" order for the user
            var orders = (await orderService.GetAllOrdersAsync())
                .Where(p => p.CurrentStatus != ModelsB.Order_B.Status.InCart && p.ApplicationUserId == userId);

            if (orders == null)
            {
                return NotFound("You have no products in the cart.");
            }
            //var inCartOrder = await orderService.GetOrderByIdAsync(order.Id);

            // Map OrderItems to a response DTO if needed
            var orderItems = new List<object>();
            foreach (var order in orders)
            {
               
                var list = new List<SelectOrderItemBDTO>();
                foreach (var ele in order.OrderItems)
                {
                    if (ele.IsDeleted == false) list.Add(ele);
                }
                order.OrderItems = list;
                 
                //var items = await orderItemService.GetAllItemsOfOrderAsync(order.Id);
                if (order.OrderItems.Any())
                {
                    orderItems.Add(new
                    {
                        Id = order.Id,
                        TotalAmount = order.TotalPrice,
                        Status = order.CurrentStatus.ToString(),
                        OrderDate = order.OrderDate.ToString(),
                        Items = order.OrderItems.Select(i => new
                        {
                            productName = i.ProductName,
                            Quantity = i.Quantity,
                            Price = i.Price
                        }).ToList()
                    });
                }
            }

            return Ok(orderItems);
        }
    }
}


