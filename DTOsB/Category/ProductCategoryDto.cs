using DTOsB.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOsB.Category
{
    public class ProductCategoryDto
    {
        public int ProductId { get; set; }
        public ProductDto Product { get; set; }
        public int CategoryId { get; set; }
        public GetAllCategoriesDTO Category { get; set; }
        public bool IsMainCategory { get; set; }
    }
}
