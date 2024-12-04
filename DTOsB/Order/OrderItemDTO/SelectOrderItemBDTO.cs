using ModelsB.Product_B;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOsB.Order.OrderItemDTO
{
    public class SelectOrderItemBDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public int StockQuantity { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public ProductB Product { get; set; }

        public string Url { get; set; }
        public string BrandName { get; set; }


        //****************************
        [MaxLength(100)]
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        [MaxLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime Updated { get; set; } = DateTime.Now;
        public bool? IsDeleted { get; set; } = false;
    }
}
