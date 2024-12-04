using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOsB.Order.PaymentDTO
{
    public class AddOrUpdatePaymentBDTO

    { 
     public int Id { get; set; }

    [Column(TypeName = "money")]
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.Now;
    [MaxLength(50)]
    public string PaymentMethod { get; set; } = "PayPal";
    [MaxLength(50)]
    public string PaymentStatus { get; set; } = "Success";  // e.g., "Success", "Failed"
    [MaxLength(100)]
    public string TransactionId { get; set; }
    [MaxLength(500)]
    public string GatewayResponse { get; set; } = "wait to know";
    public int OrderId { get; set; }


        [MaxLength(100)]
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        [MaxLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime Updated { get; set; } = DateTime.Now;
        public bool? IsDeleted { get; set; } = false;
    }
}
