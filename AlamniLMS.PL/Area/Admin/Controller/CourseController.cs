using AlamniLMS.BLL.Services.Classes;
using AlamniLMS.BLL.Services.Interfacese;
using AlamniLMS.DAL.DTO.Requests;
using AlamniLMS.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlamniLMS.PL.Area.Admin.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Admin")]
    //[Authorize(Roles = "Admin,SuperAdmin")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet("")]
        public  IActionResult GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize =5) 

        {
           var Courses =  _courseService.GetAllCourses(Request, pageNumber , pageSize , false);

             return Ok(Courses);
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromForm] CourseRequest request )
        {
            var result  = await _courseService.CreateCourse(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var Course = _courseService.GetById(id);
            if (Course == null)
            {
                return NotFound();
            }
            return Ok(Course);
        }


  


        // PATCH:  update  api/Courses/
        [HttpPatch("{id}")]
        
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] CourseRequest request)
        {
            var result = await _courseService.UpdateFile(id, request);

            if (result <= 0)
            {
                return BadRequest("Failed to update a brand");
            }

            return Ok(new { message = "Course updated successfully" });
        }





        [HttpPatch("ToggleStatus/{id}")]
        public IActionResult ToggleStatus([FromRoute] int id)
        {
            var result = _courseService.ToggleStatus(id);
            return result ? Ok(new { message = "Status Toggled" }) : NotFound(new { message = "Status not Toggled" });
        }




     

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id) // نحولها إلى Async
        {
            var result = await _courseService.DeleteCourse(id); // <--- استدعاء الدالة الجديدة

            if (result > 0)
            {
                return Ok(new { message = "Course deleted successfully" });
            }
            else
            {
                return NotFound(new { message = "Course not found or failed to delete" });
            }
        }
    }
}
