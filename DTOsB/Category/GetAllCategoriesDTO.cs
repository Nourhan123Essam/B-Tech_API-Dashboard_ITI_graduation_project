using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOsB.Category
{
    public class GetAllCategoriesDTO
    {
        public int Id { get; set; }
        [MaxLength(250)]
        public string? ImageUrl { get; set; }
        public ICollection<CategoryTranslationDTO> Translations { get; set; }
        public bool IsDeleted { get; set; }
        //public bool IsMainCategory { get; set; }
    }
}
