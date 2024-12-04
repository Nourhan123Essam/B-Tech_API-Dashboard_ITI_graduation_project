using ModelsB.Product_B;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsB.Category_B
{
    public class ProductCategoryB
    {
        public int ProductId { get; set; }
        public ProductB Product { get; set; }

        public int CategoryId { get; set; }
        public CategoryB Category { get; set; }

        public bool IsMainCategory { get; set; }
    }
}
