using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.DAL.Models
{
    public class CourseImage
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string ImageName { get; set; }

        public Course Course { get; set; }
    }
}
