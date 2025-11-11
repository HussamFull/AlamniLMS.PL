using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.DAL.Models
{
    public class Certificate : BaseModel
    {
        public string UserId { get; set; }
        public int CourseId { get; set; }
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
        public string FilePath { get; set; }           // مسار ملف الـ PDF
        public string CertificateNumber { get; set; }  // رقم فريد للشهادة
        public bool IsRevoked { get; set; } = false;

        // Navigation
        public Course Course { get; set; }
        // public ApplicationUser User { get; set; } // اختياري
    }
}
