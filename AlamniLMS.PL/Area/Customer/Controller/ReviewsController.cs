using AlamniLMS.BLL.Services.Interfacese;
using AlamniLMS.DAL.DTO.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
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
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ReviewsController(IReviewService reviewService , IStringLocalizer<SharedResource> localizer)
        {
            _reviewService = reviewService;
            _localizer = localizer;
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
                return BadRequest(_localizer["You cannot review this Course."]);
            }
            return Ok(_localizer["Review added successfully."]);
        }
    }

}
