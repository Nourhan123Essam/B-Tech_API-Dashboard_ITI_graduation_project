using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsB.Product_B
{
    public class ProductSpecificationsB
    {
        public int Id { get; set; }

        [ForeignKey("ProductB")]
        public int ProductId { get; set; }
        public ProductB Product { get; set; }
        public ICollection<ProductSpecificationTranslationB> Translations { get; set; }
    }
}
