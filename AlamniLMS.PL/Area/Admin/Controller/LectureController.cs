using AlamniLMS.BLL.Services.Interfacese;
using AlamniLMS.DAL.DTO.Requests;
using AlamniLMS.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AlamniLMS.PL.Area.Admin.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Admin")]
    //[Authorize(Roles = "Admin,SuperAdmin")]
    public class LecturesController : ControllerBase
    {
        private readonly ILectureService _lectureService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public LecturesController(ILectureService lectureService, IStringLocalizer<SharedResource> localizer)
        {
            _lectureService = lectureService;
            _localizer = localizer;
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            // **✅ التعديل هنا:** تمرير Request
            var lectures = _lectureService.GetAllLectures(Request);
            return Ok(lectures);
        }

        //[HttpGet("")]
        //public  IActionResult GetAll()=> Ok(_lectureService.GetAll());


        [HttpPost("")]
        public async Task<IActionResult> Create([FromForm] LectureRequest request )
        {
            var result  = await _lectureService.CreateFile(request);
            return CreatedAtAction(
                nameof(GetById),
                new { id = result }, // وسائط المسار: لتحديد موقع الكيان الجديد (الـ ID)
                new { message = _localizer["Lecture added successfully"].Value } // جسم الاستجابة: هنا نضع الرسالة
           );
        }

        // 123  GET: api/Lectures/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            // **✅ التعديل هنا:** استدعاء دالة الخدمة الجديدة التي تقبل Request
            var lecture = _lectureService.GetLectureById(id, Request);
            if (lecture == null)
                return NotFound();

            return Ok(lecture);
        }

        //[HttpGet("{id}")]
        //public IActionResult GetById(int id)
        //{
        //    var lecture = _lectureService.GetLectureById(id);
        //    if (lecture == null)
        //        return NotFound();

        //    return Ok(lecture);
        //}

        // update

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] LectureRequest request)
        {
            var result = await _lectureService.UpdateLecture(id, request);

            if (result > 0)
                return Ok(new { message = _localizer["Lecture updated successfully"] });

            return NotFound(new { message = _localizer["Lecture not found"] });
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _lectureService.DeleteLecture(id);

            if (result > 0)
                return Ok(new { message = _localizer["Lecture deleted successfully"] });

            return NotFound(new { message = _localizer["Lecture not found"] });
        }





    }
}
