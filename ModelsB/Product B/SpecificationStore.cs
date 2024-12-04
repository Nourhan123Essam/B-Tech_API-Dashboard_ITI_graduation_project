using ModelsB.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsB.Product_B
{
    public class SpecificationStore: BaseTranslationB
    {
     
        [MaxLength(100)]
        public string SpecKeys { get; set; }
    }
}
