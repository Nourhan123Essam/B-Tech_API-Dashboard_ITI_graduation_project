using DTOsB.Product;
using DTOsB.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationB.Services_B.Product
{
    public interface IReviewService
    {
        Task<ResultView<ReviewDto>> AddReviewAsync(ReviewDto reviewDto);
        Task<ResultView<ReviewDto>> UpdateReviewAsync(ReviewDto reviewDto);
        Task<ResultView<ReviewDto>> DeleteReviewAsync(int id);
        Task<ResultView<ReviewDto>> GetReviewByIdAsync(int id);
        Task<IEnumerable<ReviewDto>> GetAllReviewsAsync();
    }
}
