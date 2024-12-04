using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsB.Authentication_and_Authorization_B;

namespace ModelsB.Product_B
{
    public class ReviewB
    {
        public int Id { get; set; }
        [Range(1, 5)]
        public int? Rating { get; set; }  // 1-5 stars
        [MaxLength(200)]
        public string? Comment { get; set; }
        public DateTime DatePosted { get; set; } = DateTime.Now;
        [ForeignKey("ProductB")]
        public int ProductId { get; set; } 
        public ProductB Product { get; set; }
        
        [ForeignKey("ApplicationUserB")]
        public string ApplicationUserId { get; set; }
        public ApplicationUserB ApplicationUser { get; set; }

    }
}
