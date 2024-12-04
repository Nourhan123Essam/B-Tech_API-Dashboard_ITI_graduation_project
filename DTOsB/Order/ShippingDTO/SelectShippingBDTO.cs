using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOsB.Order.ShippingDTO
{
    public class SelectShippingBDTO
    {
        public string ShippingAddress { get; set; }

        public string ShippingMethod { get; set; }  // e.g., "Standard", "Express"

        public decimal ShippingCost { get; set; }
        public DateTime ShippedDate { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }

        public decimal OrderTotalPrice { get; set; }
    }
}
