using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.DAL.Models
{
    public class Brand : BaseModel
    {
        public string Name { get; set; }

        public string MainImage { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public List<Course> Courses { get; set; } = new List<Course>();

    }
}
