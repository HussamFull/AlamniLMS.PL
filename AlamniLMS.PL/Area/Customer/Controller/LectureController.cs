using AlamniLMS.BLL.Services.Interfacese;
using AlamniLMS.DAL.DTO.Requests;
using AlamniLMS.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AlamniLMS.PL.Area.Customer.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Customer")]
    //[Authorize(Roles = "Customer")]
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
        public  IActionResult GetAll()=> Ok(_lectureService.GetAll());


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
            var lecture = _lectureService.GetLectureById(id);
            if (lecture == null)
                return NotFound();

            return Ok(lecture);
        }

       





    }
}
