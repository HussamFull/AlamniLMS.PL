using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.DAL.DTO.Requests
{
    public class EnrollmentRequest
    {
        public int CourseId { get; set; }

        public IFormFile? ThumbnailPath { get; set; }   // مسار الصورة المصغرة

    }
}
