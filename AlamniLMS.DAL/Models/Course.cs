using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.DAL.Models
{
    public class Course : BaseModel
    {
        public string Title { get; set; }           // عنوان الكورس
        public string? Slug { get; set; }            // اختياري: للاستخدام في الروابط الصديقة
        public string? ShortDescription { get; set; }
        public string? FullDescription { get; set; }
        public decimal Price { get; set; }          // سعر الكورس
        public string? ThumbnailPath { get; set; }   // مسار الصورة المصغرة

        public int CategoryId { get; set; }         // إن كنت تستخدم فئات؛ يمكن أن تكون FK
        public bool IsPublished { get; set; } = false;
        public DateTime? UpdatedAt { get; set; }

        // التنقلات
        public List<Lecture> Lectures { get; set; } = new List<Lecture>();
        public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public List<Certificate> Certificates { get; set; } = new List<Certificate>();
        public List<CourseImage> SubImages { get; set; } = new List<CourseImage>();

        public List<Review> Reviews { get; set; } = new List<Review>();


    }
}
