using AlamniLMS.BLL.Services.Classes;
using AlamniLMS.BLL.Services.Interfacese;
using AlamniLMS.DAL.DTO.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlamniLMS.PL.Area.Customer.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Customer")]
   // [Authorize(Roles = "Customer")]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet("")]
        //public IActionResult GetAll()
        //{
        //    var Brand = _brandService.GetAll(true);
        //    return Ok(Brand);
        //}
        public IActionResult GetAll()
        {
            var brands = _brandService.GetAll(false);

            // تعديل النتيجة لتحتوي رابط الصورة
            var result = brands.Select(b => new
            {
                b.Id,
                b.Name,
                MainImageUrl = string.IsNullOrEmpty(b.MainImage)
                    ? null
                   : Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", b.MainImage)
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        //public IActionResult GetById([FromRoute] int id)
        //{
        //    var Brand = _brandService.GetById(id);
        //    if (Brand == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(Brand);
        //}
        public IActionResult GetById([FromRoute] int id)
        {
            var brand = _brandService.GetById(id);
            if (brand is null)
                return NotFound();

            var result = new
            {
                brand.Id,
                brand.Name,
                MainImagePath = string.IsNullOrEmpty(brand.MainImage)
                    ? null
                    : Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", brand.MainImage)
            };

            return Ok(result);
        }

    }
}
