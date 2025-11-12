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
   // [Authorize(Roles = "Admin,SuperAdmin")]


    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        // GET: api/Brands
        [HttpGet("")]
        public IActionResult GetAll()
        {
            var brands = _brandService.GetAll(false);

            // تعديل النتيجة لتحتوي رابط الصورة
            //var result = brands.Select(b => new
            //{
            //    b.Id,
            //    b.Name,
            //    MainImageUrl = string.IsNullOrEmpty(b.MainImage)
            //        ? null
            //       : Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", b.MainImage)
            //});

            return Ok(brands);
            //var Brand = _brandService.GetAll();
            //return Ok(Brand);
        }

        // GET: api/Brands/5

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var Brand = _brandService.GetById(id);
            if (Brand == null)
            {
                return NotFound();
            }
            //var result = new
            //{
            //    Brand.Id,
            //    Brand.Name,
            //    MainImagePath = string.IsNullOrEmpty(Brand.MainImage)
            //     ? null
            //     : Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", Brand.MainImage)
            //};
            return Ok(Brand);
        }


        // POST:Create  api/Brands

        [HttpPost("")]
        public async Task<IActionResult> Create([FromForm] BrandRequest request)
        {
            var result = await _brandService.CreateFile(request);

            // إضافة فحص لنتيجة العملية
            if (result <= 0)
            {
                return BadRequest("Failed to create a brand");
            }
            //public IActionResult Create([FromBody] BrandRequest request)
            //{
            //    var newCategoryId = _brandService.Create(request);

            return CreatedAtAction(
                 nameof(GetById),
                 new { id = result }, // وسائط المسار: لتحديد موقع الكيان الجديد (الـ ID)
                 new { message = "Brand added successfully" } // جسم الاستجابة: هنا نضع الرسالة
            );
        }


        // PATCH:  update  api/Brands/5  
        [HttpPatch("{id}")]
        //public IActionResult Update([FromRoute] int id, [FromBody] BrandRequest request)
        //{
        //    var result = _brandService.Update(id, request);
        //    return result > 0 ? Ok(new { message = "Brand updateed successfully" }) : NotFound();
        //}
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] BrandRequest request)
        {
            var result = await _brandService.UpdateFile(id, request);

            if (result <= 0)
            {
                return BadRequest("Failed to update a brand");
            }

            return Ok(new { message = "Brand updated successfully" });
        }







        // PATCH: Toggle Status api/Brands/5
        [HttpPatch("ToggleStatus/{id}")]
        public IActionResult ToggleStatus([FromRoute] int id)
        {
            var result = _brandService.ToggleStatus(id);
            return result ? Ok(new { message = "Status Toggled" }) : NotFound(new { message = "Status not Toggled" });
        }




        [HttpDelete("{id}")]
        //public IActionResult Delete([FromRoute] int id)
        //{
        //    var result = _brandService.Delete(id);
        //    return result > 0 ? Ok(new { message = "Delete is Brand" }) : NotFound(new { message = "Delete not Brand" });
        //}
        public IActionResult Delete([FromRoute] int id)
        {
            // أولاً: جلب البراند للحصول على مسار الصورة
            var brand = _brandService.GetById(id);
            if (brand is null)
                return NotFound("Brand not found");

            // ثانياً: حذف الصورة من مجلد wwwroot/images إذا كانت موجودة
            if (!string.IsNullOrEmpty(brand.MainImage)) // تأكد من اسم الخاصية
            {
                // مسار المجلد الفعلي
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", brand.MainImage);

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            // ثالثاً: حذف السجل من قاعدة البيانات
            var deleted = _brandService.Delete(id);
            if (deleted <= 0)
                return BadRequest("Delete failed from database");

            return Ok(new { message = "Brand and image deleted successfully" });
        }
    }
}
