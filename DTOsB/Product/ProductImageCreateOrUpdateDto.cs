using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOsB.Product
{
    public class ProductImageCreateOrUpdateDto
    {
        public int Id { get; set; }
        public IFormFile ImageFile { get; set; } // For file upload
        public string Url { get; set; }

        public int ProductId { get; set; }
    }
}
