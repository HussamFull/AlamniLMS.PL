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
    public class LecturesController : ControllerBase
    {
        private readonly ILectureService _lectureService;

        public LecturesController(ILectureService lectureService)
        {
            _lectureService = lectureService;
        }

        [HttpGet("")]
        public  IActionResult GetAll()=> Ok(_lectureService.GetAll());


        [HttpPost("")]
        public async Task<IActionResult> Create([FromForm] LectureRequest request )
        {
            var result  = await _lectureService.CreateFile(request);
            return Ok(result);
        }

        //[HttpGet("{id}")]
        //public IActionResult GetById([FromRoute] int id)
        //{
        //    var Lecture = _lectureService.GetById(id);
        //    if (Lecture == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(Lecture);
        //}


        //[HttpPost("")]
        //public IActionResult Create([FromBody] LectureRequest request)
        //{
        //    var newCategoryId = _lectureService.Create(request);

        //    return CreatedAtAction(
        //         nameof(GetById),
        //         new { id = newCategoryId }, // وسائط المسار: لتحديد موقع الكيان الجديد (الـ ID)
        //         new { message = "Lecture added successfully" } // جسم الاستجابة: هنا نضع الرسالة
        //    );
        //}

        //[HttpPatch("{id}")]
        //public IActionResult Update([FromRoute] int id, [FromBody] LectureRequest request)
        //{
        //    var result = _lectureService.Update(id, request);
        //    return result > 0 ? Ok(new { message = "Lecture updateed successfully" }) : NotFound();
        //}

        //[HttpPatch("ToggleStatus/{id}")]
        //public IActionResult ToggleStatus([FromRoute] int id)
        //{
        //    var result = _lectureService.ToggleStatus(id);
        //    return result ? Ok(new { message = "Status Toggled" }) : NotFound(new { message = "Status not Toggled" });
        //}




        //[HttpDelete("{id}")]
        //public IActionResult Delete([FromRoute] int id)
        //{
        //    var result = _lectureService.Delete(id);
        //    return result > 0 ? Ok(new { message = "Delete is Lecture" }) : NotFound(new { message = "Delete not Lecture" });
        //}
    }
}
