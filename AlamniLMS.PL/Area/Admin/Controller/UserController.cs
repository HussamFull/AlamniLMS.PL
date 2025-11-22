using AlamniLMS.BLL.Services.Interfacese;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AlamniLMS.DAL.DTO.Requests;
using Microsoft.AspNetCore.Http;





namespace AlamniLMS.PL.Area.Admin.Controller
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Admin")]
    // [Authorize(Roles = "Admin,SuperAdmin")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        // Get All Users
        [HttpGet("")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();

            return Ok(users);
        }

        // Get User By Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] string id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // Block User
        [HttpPatch("block/{userId}")]
        public async Task<IActionResult> BlockUser([FromRoute] string userId, [FromBody] int days)
        {
            var result = await _userService.BlockUserAsync(userId, days);
            if (!result)
            {
                return NotFound();
            }
            return Ok(new { Message = "User blocked successfully" });
        }

        // UnBlock User
        [HttpPatch("unblock/{userId}")]
        public async Task<IActionResult> UnBlockUser([FromRoute] string userId)
        {
            var result = await _userService.UnBlockUserAsync(userId);
            if (!result)
            {
                return NotFound();
            }
            return Ok(new { Message = "User unblocked successfully" });
        }

        // Is Blocked User
        [HttpPatch("isblock/{userId}")]
        public async Task<IActionResult> IsBlockedUser([FromRoute] string userId)
        {
            var result = await _userService.IsBlockedAsync(userId);
            return Ok(result);
        }

        [HttpPatch("changerole/{userId}")]
       // [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ChangeRole([FromRoute] string userId, [FromBody] ChangeRoleRequest request)
        {
            var result = await _userService.ChangeUserRoleAsync(userId, request.RoleName);
            if (!result)
            {
                return NotFound();
            }
            return Ok(new { Message = "User role changed successfully" });
        }
    }
}
