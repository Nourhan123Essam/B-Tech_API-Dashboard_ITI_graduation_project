using DTOsB.Product;
using DTOsB.Shared;
using ModelsB.Product_B;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationB.Services_B.Product
{
    public interface IProductImageService
    {
        Task<ResultView<ProductImageCreateOrUpdateDto>> AddImageAsync(ProductImageCreateOrUpdateDto productImageDto);
        Task<ResultView<ProductImageCreateOrUpdateDto>> UpdateImageAsync(ProductImageCreateOrUpdateDto productImageDto);
        Task<ResultView<ProductImageDto>> GetProductImageByIdAsync(int id);
        Task<ResultView<List<ProductImageDto>>> GetProductImagesByProductIdAsync(int productId);
    }
}
