using AlamniLMS.BLL.Services.Interfacese;
using AlamniLMS.DAL.DTO.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AlamniLMS.PL.Area.Admin.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Admin")]
   // [Authorize(Roles = "Admin,SuperAdmin")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CategoriesController(ICategoriesService categoriesService, IStringLocalizer<SharedResource> localizer)
        {
            _categoriesService = categoriesService;
            _localizer = localizer;
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            var categories = _categoriesService.GetAll();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute]int id)
        {
            var category = _categoriesService.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost("")]
        public IActionResult Create([FromBody] CategoriesRequest request)
        {
            var newCategoryId = _categoriesService.Create(request);

            return CreatedAtAction(
                 nameof(GetById),
                 new { id = newCategoryId }, // وسائط المسار: لتحديد موقع الكيان الجديد (الـ ID)
                 new { message = _localizer["Category added successfully"] } // جسم الاستجابة: هنا نضع الرسالة
            );
        }

        [HttpPatch("{id}")]
        public IActionResult Update([FromRoute]int id, [FromBody] CategoriesRequest request)
        {
            var result = _categoriesService.Update(id, request);
            return result > 0 ? Ok(new { message = _localizer["Category updateed successfully"] }) : NotFound();
        }

        [HttpPatch("ToggleStatus/{id}")]
        public IActionResult ToggleStatus([FromRoute] int id)
        {
            var result = _categoriesService.ToggleStatus(id);
            return result  ? Ok( new { message = _localizer["Status Toggled"] }) : NotFound(new { message = _localizer["Status not Toggled"] });
        }



        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute]int id)
        {
            var result = _categoriesService.Delete(id);
            return result > 0 ? Ok(new { message = _localizer["Delete is Categories" ]}) : NotFound(new { message = _localizer["Delete not Categories"] });
        }
    }
}
