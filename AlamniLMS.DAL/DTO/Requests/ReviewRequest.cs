using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.DAL.DTO.Requests
{
    public class ReviewRequest
    {
        public int CourseId { get; set; }
        public string? Comment { get; set; }
        public int Rate { get; set; }

    }

}
