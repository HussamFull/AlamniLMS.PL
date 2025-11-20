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

    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;
        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpPost("")]
        public async Task<IActionResult> AddToEnrollment([FromBody] EnrollmentRequest request)
        {
            // افترض أن لديك طريقة للحصول على UserId من السياق الحالي
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _enrollmentService.AddToEnrollmentAsync(request, userId);
            if (result)
            {
                return Ok(new { Message = "تمت الإضافة إلى التسجيل بنجاح." });
            }
            else
            {
                return BadRequest(new { Message = "فشل في الإضافة إلى التسجيل." });
            }
        }

        [HttpGet("")]
        public async Task<IActionResult> GetUserEnrollment()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var enrollmentSummary = await _enrollmentService.EnrollmentSummaryResponseAsync(userId);
            return Ok(enrollmentSummary);
        }
    }
}
