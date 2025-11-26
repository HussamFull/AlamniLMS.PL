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
                        return _context.Courses
                            .Include(p => p.SubImages)
                                .Include(p => p.Reviews).ThenInclude(r => r.User)
                                    .ToList();
        }

        public async Task<Course> GetCourseWithSubImagesAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.SubImages) // <--- هذا هو الجزء الأساسي لحل المشكلة
                .Include(p => p.Reviews)
                  .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Course> GetAsync(int id)
        {
            return await _context.Courses.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Course?> GetCourseWithImagesAsync(int id)
        {
            return await _context.Courses
               .Include(p => p.SubImages) // تحميل الصور الفرعية
               .Include(p => p.Reviews) // تحميل المراجعات
                   .ThenInclude(r => r.User) // تحميل المستخدمين المرتبطين بالمراجعات
               .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}

