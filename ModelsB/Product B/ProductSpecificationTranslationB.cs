using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsB.Shared;

namespace ModelsB.Product_B
{
    public class ProductSpecificationTranslationB: BaseTranslationB
    {
        [MaxLength(100)]
        public string TranslatedKey { get; set; }

        [MaxLength(250)]
        public string TranslatedValue { get; set; }

        [ForeignKey("ProductSpecificationB")]
        public int SpecificationId { get; set; }

        public ProductSpecificationsB ProductSpecification { get; set; }
    }
}
