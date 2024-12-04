using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsB.Order_B
{
    public class OrderDiscountB
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        [ForeignKey("OrderB")]
        public OrderB Order { get; set; }

        public int DiscountId { get; set; }
        [ForeignKey("DiscountB")]
        public DiscountB Discount { get; set; }
    }
}
