using ModelsB.Category_B;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOsB.Category
{
    public class MainCategoryWithSubCategoriesDTO
    {
        public CategoryB MainCategory { get; set; }
        public List<CategoryB> SubCategories { get; set; }
    }
}
