using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsB.Product_B
{
    public class ProductImageB
    {
        public int Id { get; set; }
        [MaxLength(250)]
        public string Url { get; set; }
        [ForeignKey("ProductB")]
        public int ProductId { get; set; }
        public ProductB Product { get; set; }
    }
}
