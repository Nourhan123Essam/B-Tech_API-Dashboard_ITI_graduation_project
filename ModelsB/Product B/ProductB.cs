using ModelsB.Category_B;
using ModelsB.Order_B;
using ModelsB.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsB.Product_B
{
    public class ProductB : BaseEntityB
    {
        
        [Column(TypeName = "money")]
        [Range(0, double.MaxValue, ErrorMessage = "Product price must be a positive value.")]
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public ICollection<ProductCategoryB> ProductCategories { get; set; }
        public ICollection<ProductImageB> Images { get; set; }
        public ICollection<OrderItemB> OrderItems { get; set; }
        public ICollection<ProductSpecificationsB> Specifications { get; set; }
        public ICollection<ProductTranslationB> Translations { get; set; }
    }
}
