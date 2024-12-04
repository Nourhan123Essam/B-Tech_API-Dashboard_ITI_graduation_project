using DTOsB.Order.OrderItemDTO;
using ModelsB.Order_B;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOsB.Order.OrderDTO
{
    public class SelectOrderBDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }

        public decimal TotalPrice { get; set; }

        public Status CurrentStatus { get; set; } = Status.InCart;

        public string ApplicationUserName { get; set; }

        public decimal ShippingCost { get; set; }

        public string PaymentStatus { get; set; }
        public string ApplicationUserId { get; set; }

        public IEnumerable<SelectOrderItemBDTO> OrderItems { get; set; }


        //***********************************
        [MaxLength(100)]
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        [MaxLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime Updated { get; set; } = DateTime.Now;
        public bool? IsDeleted { get; set; } = false;
    }
}
//public enum Status
//{
//    [Display(Name = "In Cart")]
//    InCart,
//    [Display(Name = "Pending")]
//    Pending,
//    [Display(Name = "Shipped")]
//    Shipped,
//    [Display(Name = "Delivered")]
//    Delivered,
//    [Display(Name = "Cancelled")]
//    Cancelled
//}
