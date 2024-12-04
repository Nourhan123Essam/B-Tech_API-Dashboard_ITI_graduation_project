using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOsB.Product
{
    public class ReviewDto
    {
        public int Id { get; set; }
        [Range(1, 5)]
        public int? Rating { get; set; } 
        [MaxLength(200)]
        public string? Comment { get; set; }
        public DateTime DatePosted { get; set; } = DateTime.Now;

        public int ProductId { get; set; }
        public string ApplicationUserId { get; set; } 
    }
}
