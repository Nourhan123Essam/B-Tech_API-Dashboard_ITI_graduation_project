using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOsB.Product
{
    public class ProductDto
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        public string CreatedBy { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime Updated { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }
        public List<ProductImageDto> Images { get; set; }
        public List<ProductTranslationDto> Translations { get; set; }
        public List<ProductSpecificationDto> Specifications { get; set; }
    }
}
