using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOsB.Category
{
    public class CreateCategoryTranslationDto
    {
        [MaxLength(50, ErrorMessage = "Category Name cannot be longer than 50 characters.")]
        [MinLength(2, ErrorMessage = "Category Name cannot be less than 2 characters.")]
        [Required(ErrorMessage = "Category Name is required.")]
        public string CategoryName { get; set; }
        [MaxLength(150, ErrorMessage = "Description cannot be longer than 50 characters.")]
        [MinLength(20, ErrorMessage = "Description cannot be less than 20 characters.")]
        [Required(ErrorMessage = "Description is required.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Please select a language.")]
        public int? LanguageId { get; set; }
        public bool IsMainCategory { get; set; }
    }
}
