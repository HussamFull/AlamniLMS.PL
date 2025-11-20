using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.DAL.DTO.Responses
{
    public class EnrollmentSummaryResponse
    {
        public List<EnrollmentResponse> Items { get; set; } = new List<EnrollmentResponse>();
        public decimal EnrollmentToltal => Items.Sum(i => i.TotalPrice);

    }
}
