using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.DAL.DTO.Requests
{
    public class CourseRequest
    {
        public string Title { get; set; }           // عنوان الكورس
        public string? Slug { get; set; }            // اختياري: للاستخدام في الروابط الصديقة
        public string? ShortDescription { get; set; }
        public string? FullDescription { get; set; }
        public decimal Price { get; set; }          // سعر الكورس
        public IFormFile ThumbnailPath { get; set; }   // مسار الصورة المصغرة



        public int CategoryId { get; set; }         // إن كنت تستخدم فئات؛ يمكن أن تكون FK
        public int? BrandId { get; set; }
        public bool IsPublished { get; set; } = false;
        public DateTime? UpdatedAt { get; set; }

    }
}
