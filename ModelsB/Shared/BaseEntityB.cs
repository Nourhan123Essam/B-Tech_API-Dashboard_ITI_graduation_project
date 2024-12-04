using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsB.Shared
{
    public class BaseEntityB
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        [MaxLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime Updated { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }
    }
}
