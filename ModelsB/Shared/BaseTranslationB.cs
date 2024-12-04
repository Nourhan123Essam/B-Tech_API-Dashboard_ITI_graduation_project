using ModelsB.Localization_B;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsB.Shared
{
    public class BaseTranslationB
    {
        public int Id { get; set; }
        [ForeignKey("LanguageB")]
        public int LanguageId { get; set; }
        public LanguageB Language { get; set; }
    }
}
