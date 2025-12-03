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
        public IActionResult GetAll()
        {
            // **✅ التعديل هنا:** تمرير Request
            var lectures = _lectureService.GetAllLectures(Request);
            return Ok(lectures);
        }



        // 123  GET: api/Lectures/5
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







    }
}
