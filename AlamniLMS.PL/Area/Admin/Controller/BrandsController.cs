using AlamniLMS.BLL.Services.Interfacese;
using AlamniLMS.DAL.DTO.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlamniLMS.PL.Area.Admin.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            var Brand = _brandService.GetAll();
            return Ok(Brand);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var Brand = _brandService.GetById(id);
            if (Brand == null)
            {
                return NotFound();
            }
            return Ok(Brand);
        }


        [HttpPost("")]
        public IActionResult Create([FromBody] BrandRequest request)
        {
            var newCategoryId = _brandService.Create(request);

            return CreatedAtAction(
                 nameof(GetById),
                 new { id = newCategoryId }, // وسائط المسار: لتحديد موقع الكيان الجديد (الـ ID)
                 new { message = "Brand added successfully" } // جسم الاستجابة: هنا نضع الرسالة
            );
        }

        [HttpPatch("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] BrandRequest request)
        {
            var result = _brandService.Update(id, request);
            return result > 0 ? Ok(new { message = "Brand updateed successfully" }) : NotFound();
        }

        [HttpPatch("ToggleStatus/{id}")]
        public IActionResult ToggleStatus([FromRoute] int id)
        {
            var result = _brandService.ToggleStatus(id);
            return result ? Ok(new { message = "Status Toggled" }) : NotFound(new { message = "Status not Toggled" });
        }




        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var result = _brandService.Delete(id);
            return result > 0 ? Ok(new { message = "Delete is Brand" }) : NotFound(new { message = "Delete not Brand" });
        }
    }
}
