using AlamniLMS.BLL.Services.Classes;
using AlamniLMS.BLL.Services.Interfacese;
using AlamniLMS.DAL.DTO.Requests;
using AlamniLMS.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlamniLMS.PL.Area.Customer.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Customer")]
    //[Authorize(Roles = "Customer")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        //[HttpGet("")]
        //public  IActionResult GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize =5) 

        //{
        //   var Courses =  _courseService.GetAllCourses(Request, pageNumber , pageSize , false);

        //     return Ok(Courses);
        //}

        [HttpGet("")]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        {
            // استدعاء مباشر للدالة في السيرفس مع await، دون أي معالجة إضافية للبيانات
            var courses = await _courseService.GetAllCourses(Request, pageNumber, pageSize, false);

            return Ok(courses);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id) // يفضل أن تكون async
        {
            // ملاحظة: ستحتاج إلى دالة GetCourseByIdWithImages في الـ Service
            var course = await _courseService.GetCourseByIdWithImages(id, Request);

            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }







    }
}
