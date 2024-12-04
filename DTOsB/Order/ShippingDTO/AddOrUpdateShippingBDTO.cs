using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOsB.Order.ShippingDTO
{
    public class AddOrUpdateShippingBDTO
    {
        public int Id { get; set; }

        [MaxLength(250)]
        public string ShippingAddress { get; set; }

        [MaxLength(50)]
        public string ShippingMethod { get; set; } = "Standard";  // e.g., "Standard", "Express"

        [Column(TypeName = "money")]
        public decimal ShippingCost { get; set; }
        public DateTime ShippedDate { get; set; } = DateTime.Now;
        public DateTime EstimatedDeliveryDate { get; set; } = new DateTime(DateTime.Now.Second + 2);

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
