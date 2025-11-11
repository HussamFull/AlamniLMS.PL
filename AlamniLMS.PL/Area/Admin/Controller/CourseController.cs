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
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet("")]
        public  IActionResult GetAll()=> Ok(_courseService.GetAll());


        [HttpPost("")]
        public async Task<IActionResult> Create([FromForm] CourseRequest request )
        {
            var result  = await _courseService.CreateFile(request);
            return Ok(result);
        }

        //[HttpGet("{id}")]
        //public IActionResult GetById([FromRoute] int id)
        //{
        //    var Course = _courseService.GetById(id);
        //    if (Course == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(Course);
        //}


        //[HttpPost("")]
        //public IActionResult Create([FromBody] CourseRequest request)
        //{
        //    var newCategoryId = _courseService.Create(request);

        //    return CreatedAtAction(
        //         nameof(GetById),
        //         new { id = newCategoryId }, // وسائط المسار: لتحديد موقع الكيان الجديد (الـ ID)
        //         new { message = "Course added successfully" } // جسم الاستجابة: هنا نضع الرسالة
        //    );
        //}

        //[HttpPatch("{id}")]
        //public IActionResult Update([FromRoute] int id, [FromBody] CourseRequest request)
        //{
        //    var result = _courseService.Update(id, request);
        //    return result > 0 ? Ok(new { message = "Course updateed successfully" }) : NotFound();
        //}

        //[HttpPatch("ToggleStatus/{id}")]
        //public IActionResult ToggleStatus([FromRoute] int id)
        //{
        //    var result = _courseService.ToggleStatus(id);
        //    return result ? Ok(new { message = "Status Toggled" }) : NotFound(new { message = "Status not Toggled" });
        //}




        //[HttpDelete("{id}")]
        //public IActionResult Delete([FromRoute] int id)
        //{
        //    var result = _courseService.Delete(id);
        //    return result > 0 ? Ok(new { message = "Delete is Course" }) : NotFound(new { message = "Delete not Course" });
        //}
    }
}
