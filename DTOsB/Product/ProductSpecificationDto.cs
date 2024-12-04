using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOsB.Product
{
    public class ProductSpecificationDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public List<ProductSpecificationTranslationDto> Translations { get; set; }
    }
}
