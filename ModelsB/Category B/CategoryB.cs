using ModelsB.Product_B;
using ModelsB.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsB.Category_B
{
    public class CategoryB: BaseEntityB
    {
      
        [MaxLength(250)]
        public string? ImageUrl { get; set; }
        public ICollection<ProductCategoryB> ProductCategories { get; set; }
        public ICollection<CategoryTranslationB> Translations { get; set; }
    }
}
