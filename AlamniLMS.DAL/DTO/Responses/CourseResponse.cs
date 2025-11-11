using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlamniLMS.DAL.DTO.Responses
{
    public class CourseResponse
    {
        public string Title { get; set; }           // عنوان الكورس
       // public string? Slug { get; set; }            // اختياري: للاستخدام في الروابط الصديقة
        //public string? ShortDescription { get; set; }
        public string? FullDescription { get; set; }
        [JsonIgnore]
        public string ThumbnailPath { get; set; }

        public string ThumbnailPathUrl => $"https://localhost:7227/images/{ThumbnailPath}";
    }
}
