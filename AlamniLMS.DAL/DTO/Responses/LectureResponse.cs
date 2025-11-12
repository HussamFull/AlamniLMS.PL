using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.DAL.DTO.Responses
{
    public class LectureResponse
    {
       // public int CourseId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? VideoUrl { get; set; }       // أو مسار الملف في التخزين

        public string? VideoUrlUrl =>  $"https://localhost:7122/videos/{VideoUrl}";
        public int? Order { get; set; }             // ترتيب المحاضرة داخل الكورس
        public int? DurationSeconds { get; set; }   // مدة الفيديو (اختياري)

    }
}
