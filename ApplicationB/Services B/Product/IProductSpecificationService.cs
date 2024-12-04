using DTOsB.Product;
using DTOsB.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationB.Services_B.Product
{
    public interface IProductSpecificationService
    {
        Task<ResultView<IEnumerable<ProductSpecificationDto>>> GetSpecificationsByProductIdAsync(int productId);
        Task<ResultView<ProductSpecificationDto>> GetSpecificationByIdAsync(int id);
        Task<ResultView<ProductSpecificationDto>> AddSpecificationAsync(ProductSpecificationDto specificationDto);
        Task<ResultView<ProductSpecificationDto>> UpdateSpecificationAsync(ProductSpecificationDto specificationDto);
       
    }
}
