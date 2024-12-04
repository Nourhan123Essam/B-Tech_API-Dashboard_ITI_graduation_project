using ModelsB.Authentication_and_Authorization_B;
using ModelsB.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsB.Order_B
{
    public class OrderB : BaseEntityB
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        [Column(TypeName = "money")]
        [Range(0, double.MaxValue, ErrorMessage = "Total price must be a positive value.")]
        public decimal TotalPrice { get; set; }
        public Status CurrentStatus { get; set; } = Status.InCart;
        public ICollection<OrderItemB> OrderItems { get; set; }
        public ICollection<DiscountB> Discounts { get; set; }
        [ForeignKey("ApplicationUserB")]
        public string ApplicationUserId { get; set; }
        public ApplicationUserB ApplicationUser { get; set; }
        [ForeignKey("ShippingB")]
        public int ShippingId { get; set; }
        public ShippingB Shipping { get; set; }

        [ForeignKey("PaymentB")]
        public int PaymentId { get; set; }
        public PaymentB Payment { get; set; }

    }

    public enum Status
    {
        [Display(Name = "In Cart")]
        InCart,
        [Display(Name = "Pending")]
        Pending,
        [Display(Name = "Shipped")]
        Shipped,
        [Display(Name = "Delivered")]
        Delivered,
        [Display(Name = "Cancelled")]
        Cancelled
    }
}
