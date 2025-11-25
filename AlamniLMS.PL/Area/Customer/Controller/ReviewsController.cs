using AlamniLMS.BLL.Services.Interfacese;
using AlamniLMS.DAL.DTO.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AlamniLMS.PL.Area.Customer.Controller
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Customer")]
    [Authorize(Roles = "Customer")]

    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost("")]
        public async Task<IActionResult> AddReview([FromBody] ReviewRequest reviewRequest)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var result = await _reviewService.AddReviewAsync(reviewRequest, userId);
            if (!result)
            {
                return BadRequest("You cannot review this Course.");
            }
            return Ok("Review added successfully.");
        }
    }

}
