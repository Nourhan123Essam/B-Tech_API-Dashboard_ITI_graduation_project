using ApplicationB.Contracts_B.Product;
using ApplicationB.Services_B.User;
using AutoMapper;
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
    public class ReviewService: IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public ReviewService(IReviewRepository reviewRepository, IMapper mapper, IUserService userService)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<ResultView<ReviewDto>> AddReviewAsync(ReviewDto reviewDto)
        {
            var review = _mapper.Map<ReviewB>(reviewDto);
            //review.ApplicationUserId = _userService.GetCurrentUserId();
            review.DatePosted = DateTime.Now;

            await _reviewRepository.AddAsync(review);
            return ResultView<ReviewDto>.Success(reviewDto);
        }

        public async Task<ResultView<ReviewDto>> UpdateReviewAsync(ReviewDto reviewDto)
        {
            var existingReview = await _reviewRepository.GetByIdAsync(reviewDto.Id);
            if (existingReview == null || existingReview.Product.IsDeleted)
                return ResultView<ReviewDto>.Failure("Review not found.");

            _mapper.Map(reviewDto, existingReview);
            existingReview.DatePosted = DateTime.Now;

            await _reviewRepository.UpdateAsync(existingReview);
            return ResultView<ReviewDto>.Success(reviewDto);
        }

        public async Task<ResultView<ReviewDto>> DeleteReviewAsync(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null)
                return ResultView<ReviewDto>.Failure("Review not found.");

            await _reviewRepository.DeleteAsync(review.Id);
            return ResultView<ReviewDto>.Success(null);
        }

        public async Task<ResultView<ReviewDto>> GetReviewByIdAsync(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null)
                return ResultView<ReviewDto>.Failure("Review not found.");

            var reviewDto = _mapper.Map<ReviewDto>(review);
            return ResultView<ReviewDto>.Success(reviewDto);
        }

        public async Task<IEnumerable<ReviewDto>> GetAllReviewsAsync()
        {
            var reviews = await _reviewRepository.GetAllAsync();
            var reviewDtos = _mapper.Map<IEnumerable<ReviewDto>>(reviews);
            return reviewDtos;
        }
    }
}
