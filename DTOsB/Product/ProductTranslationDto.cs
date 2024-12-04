using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOsB.Product
{
    public class ProductTranslationDto
    {
        public int Id { get; set; }
        [MinLength(3, ErrorMessage = "Product Name cannot be less than 3 characters.")]
        [Required(ErrorMessage = "Product Name is required.")]
        public string Name { get; set; }

        [MinLength(3, ErrorMessage = "Brand Name cannot be less than 3 characters.")]
        [Required(ErrorMessage = "Brand Name is required.")]
        public string BrandName { get; set; }
        [MinLength(5, ErrorMessage = "Description cannot be less than 5 characters.")]
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        public int LanguageId { get; set; }

        public int ProductId { get; set; }
    }
}
