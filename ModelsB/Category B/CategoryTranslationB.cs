using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsB.Shared;

namespace ModelsB.Category_B
{
    public class CategoryTranslationB: BaseTranslationB
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string CategoryName { get; set; }

        [MaxLength(150)]
        public string? Description { get; set; }

        [ForeignKey("CategoryB")]
        public int CategoryId { get; set; }

        public CategoryB Category { get; set; }
    }
}
