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
    public class DiscountB: BaseEntityB
    {
        public int Id { get; set; }
        [MaxLength(150)]
        public string Name { get; set; }
        [MaxLength(100)]
        
        public string? Code { get; set; }// For coupons
        [MaxLength(100)]
        public string Type { get; set; } // e.g., "Percentage", "Fixed Amount""
        [Column(TypeName = "money")]
        [Range(0, double.MaxValue, ErrorMessage = "Discount must be a positive value.")]
        public decimal Amount { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsOneTimeUse { get; set; } // For coupons
        public bool AppliesToShipping { get; set; } // Whether the promotion applies to shipping costs
        public bool IsActive { get; set; } // Whether the discount is currently active
        public int MinimumPurchaseAmount { get; set; } // Applicable for coupons and promotions
        public ICollection<OrderB> Orders { get; set; }
    }
}
