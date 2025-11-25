using AlamniLMS.BLL.Services.Interfacese;
using AlamniLMS.DAL.DTO.Requests;
using AlamniLMS.DAL.Models;
using AlamniLMS.DAL.Repository.Interfaces;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.BLL.Services.Classes
{
    public class ReviewService : IReviewService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IOrderRepository orderRepository, IReviewRepository reviewRepository)
        {
            _orderRepository = orderRepository;
            _reviewRepository = reviewRepository;
        }

        public async Task<bool> AddReviewAsync(ReviewRequest reviewRequest, string userId)
        {
            // Check if the user has an approved order for the product
            var hasOrder = await _orderRepository.UserHasApprovedOrderForProductAsync(userId, reviewRequest.CourseId);
            if (!hasOrder)
            {
                return false; // User cannot review the product
            }
            var alreadyReviews = await _reviewRepository.HasUserReviewdProduct(userId, reviewRequest.CourseId);
            if (alreadyReviews)
            {
                return false; // User has already reviewed the product
            }
            var review = reviewRequest.Adapt<Review>();
            await _reviewRepository.AddReviewAsync(review, userId);
            return true;
        }
    }

}
