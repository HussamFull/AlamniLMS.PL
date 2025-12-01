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

    public class CheckOutsController : ControllerBase
    {
        private readonly ICheckOutService _checkOutService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CheckOutsController(ICheckOutService checkOutService, IStringLocalizer<SharedResource> localizer)
        {

            _checkOutService = checkOutService;
            _localizer = localizer;
        }
        [HttpPost("payment")]
        public async Task<IActionResult> Payment([FromBody] CheckOutRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var response = await _checkOutService.ProcessPaymentAsync(request, userId, Request);

            return Ok(response);
        }

        [HttpGet("Success/{orderId}")]
        //[HttpGet("Success")]
        [AllowAnonymous]
        public async Task<IActionResult> Success([FromRoute] int orderId)
            
        {
            var result = await _checkOutService.HandlePaymentSuccessAsync(orderId);
            return Ok(result + _localizer["Payment succeeded!"]);
        }

    }

}
