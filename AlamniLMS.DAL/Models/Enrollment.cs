using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.DAL.Models
{
    public class Enrollment : BaseModel
    {
        public string UserId { get; set; }         // FK إلى جدول AspNetUsers (Identity)
        public int CourseId { get; set; }
        public DateTime? EnrolledAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public decimal PaidAmount { get; set; }    // إن أردت تسجيل المبلغ المدفوع
        public double? ProgressPercent { get; set; } = 0.0; // نسبة التقدم 0-100
        public DateTime? CompletedAt { get; set; }

        // Navigation
        public Course Course { get; set; }
        // public ApplicationUser User { get; set; } // حدد نوع المستخدم إذا تريد navigation property

    }
}
