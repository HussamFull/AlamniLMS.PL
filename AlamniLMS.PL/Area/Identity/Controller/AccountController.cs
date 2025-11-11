using AlamniLMS.BLL.Services.Interfacese;
using AlamniLMS.DAL.DTO.Requests;
using AlamniLMS.DAL.DTO.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlamniLMS.PL.Area.Identity.Controller
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Identity")]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserResponse>> Register(RegisterRequest registerRequest)
        {
            //if (registerRequest == null)
            //{
            //    return BadRequest("Invalid registration request.");
            //}
            //try
            //{
            var result = await _authenticationService.RegisterAsync(registerRequest
                // , Request
                );
            return Ok(result);
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, $"Internal server error: {ex.Message}");
            //}
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserResponse>> Login(LoginRequest loginRequest)
        {
            //if (loginRequest == null)
            //{
            //    return BadRequest("Invalid login request.");
            //}
            //try
            //{
            var userResponse = await _authenticationService.LoginAsync(loginRequest);
            return Ok(userResponse);
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, $"Internal server error: {ex.Message}");
            // }
        }

        [HttpGet("ConfirmEmail")]
        public async Task<ActionResult<string>> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            //try
            //{
            var result = await _authenticationService.ConfirmEmailAsync(userId, token);
            return Ok(result);
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, $"Internal server error: {ex.Message}");
            //}
        }

        [HttpPost("ForgotPassword")]
        public async Task<ActionResult<bool>> ForgotPassword(ForgotPasswordRequest request)
        {
            //try
            //{
            var result = await _authenticationService.ForgotPasswordAsync(request);
            return Ok(result);
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, $"Internal server error: {ex.Message}");
            //}

        }
        [HttpPost("reset-password")]
        public async Task<ActionResult<string>> ResetPasswordAsync(ResetPasswordRequest request)
        {
            //try
            //{
            var result = await _authenticationService.ResetPasswordAsync(request);
            return Ok(result);
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, $"Internal server error: {ex.Message}");
            //}
        }
    }
}