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

        // GET: api/Brands
        [HttpGet("")]
        public IActionResult GetAll()
        {
            var brands = _brandService.GetAll(false);

                    return Ok(brands);
           
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
          
            return Ok(Brand);
        }


    }
}
