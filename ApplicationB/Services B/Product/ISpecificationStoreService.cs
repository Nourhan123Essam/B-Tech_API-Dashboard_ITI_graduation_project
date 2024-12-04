using DTOsB.Product;
using DTOsB.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationB.Services_B.Product
{
    public interface ISpecificationStoreService
    {
        Task<ResultView<SpecificationStoreDto>> AddReviewAsync(SpecificationStoreDto reviewDto);
        Task<ResultView<SpecificationStoreDto>> UpdateReviewAsync(SpecificationStoreDto reviewDto);
        Task<ResultView<SpecificationStoreDto>> DeleteReviewAsync(int id);
        Task<ResultView<SpecificationStoreDto>> GetReviewByIdAsync(int id);
        Task<IEnumerable<SpecificationStoreDto>> GetAllReviewsAsync();
    }
}
