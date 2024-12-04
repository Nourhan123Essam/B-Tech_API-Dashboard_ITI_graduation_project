using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOsB.Product
{
    public class ProductSpecificationTranslationDto
    {
        public int Id { get; set; }
        [MinLength(3, ErrorMessage = "Key cannot be less than 3 characters.")]
        [Required(ErrorMessage = "Key is required.")]
        public string TranslatedKey { get; set; }
        [MinLength(3, ErrorMessage = "Value cannot be less than 3 characters.")]
        [Required(ErrorMessage = "Value is required.")]
        public string TranslatedValue { get; set; }
        public int LanguageId { get; set; }
        public int SpecificationId { get; set; }
    }
}
