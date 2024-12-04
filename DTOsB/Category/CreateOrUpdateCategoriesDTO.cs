using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DTOsB.Category
{
    public class CreateOrUpdateCategoriesDTO
    {
        public string? ImageUrl { get; set; }
        public List<CreateCategoryTranslationDto> Translations { get; set; } = new List<CreateCategoryTranslationDto>();
        public ICollection<int>? ProductIds { get; set; }
    }
}
