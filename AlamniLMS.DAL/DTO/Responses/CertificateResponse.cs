using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.DAL.DTO.Responses
{
    public class CertificateResponse
    {
        public int Id { get; set; }
        public string CertificateNumber { get; set; }
        public string FilePath { get; set; }
        public string UserId { get; set; }
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string IssuedAt { get; set; }
    }
}
