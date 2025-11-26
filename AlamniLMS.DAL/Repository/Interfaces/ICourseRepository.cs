using AlamniLMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.DAL.Repository.Interfaces
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        List<Course> GetAllCoursesWithImage();

        Task<Course> GetCourseWithSubImagesAsync(int id);
        Task<Course> GetAsync(int id);

        Task<Course?> GetCourseWithImagesAsync(int id);

    }
}
