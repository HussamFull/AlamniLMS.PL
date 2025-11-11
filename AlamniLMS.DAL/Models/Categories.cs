using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.DAL.Models
{
    public class Categories : BaseModel
    {
        public string Name { get; set; } 
        public string? Description { get; set; } 

        public List<Course> Courses { get; set; } = new List<Course>();



    }
}
