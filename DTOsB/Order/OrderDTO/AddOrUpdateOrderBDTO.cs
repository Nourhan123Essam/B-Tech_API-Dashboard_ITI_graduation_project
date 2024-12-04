using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsB.Order_B;
using ModelsB.Authentication_and_Authorization_B;
using DTOsB.Order.OrderItemDTO;

namespace DTOsB.Order.OrderDTO
{
    public class AddOrUpdateOrderBDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }

        [Column(TypeName = "money")]
        [Range(0, double.MaxValue, ErrorMessage = "Total price must be a positive value.")]
        public decimal TotalPrice { get; set; }

        public Status CurrentStatus { get; set; } = Status.InCart;


        public IEnumerable<ApplicationUserB>? Users { get; set; }
        public List<AddOrUpdateOrderItemBDTO>? orderItems { get; set; } = new List<AddOrUpdateOrderItemBDTO>();


        public string ApplicationUserId { get; set; }

        public int? ShippingId { get; set; } = 0;

        public int? PaymentId { get; set; } = 0;

        [MaxLength(100)]
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        [MaxLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime Updated { get; set; } = DateTime.Now;
        public bool? IsDeleted { get; set; } = false;

    }
}
