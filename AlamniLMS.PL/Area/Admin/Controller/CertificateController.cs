using AlamniLMS.BLL.Services.Interfacese;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AlamniLMS.PL.Area.Admin.Controller
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Admin")]
   [Authorize(Roles = "Admin,SuperAdmin")]
    public class CertificateController : ControllerBase
    {
        private readonly ICertificateService _certificateService;

        public CertificateController(ICertificateService certificateService)
        {
            _certificateService = certificateService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate(int courseId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _certificateService.GenerateCertificateAsync(userId, courseId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var cert = await _certificateService.GetCertificateAsync(id, Request);
            if (cert == null) return NotFound();
            return Ok(cert);
        }
    }
}
