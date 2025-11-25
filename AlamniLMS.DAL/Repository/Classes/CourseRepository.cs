using AlamniLMS.DAL.Data;
using AlamniLMS.DAL.Models;
using AlamniLMS.DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.DAL.Repository.Classes
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;

        }
        public List<Course> GetAllCoursesWithImage()
                    {
                        return _context.Courses.Include(p => p.SubImages)
                         .Include(p => p.Reviews).ThenInclude(r => r.User)
                         .ToList();
        }
        

    }
}

