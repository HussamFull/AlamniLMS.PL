using AlamniLMS.DAL.DTO.Requests;
using AlamniLMS.DAL.DTO.Responses;
using AlamniLMS.DAL.Models;
using Microsoft.AspNetCore.Http;
using QuestPDF.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.BLL.Services.Interfacese
{
    public interface ICourseService : IGenericService<CourseRequest, CourseResponse , Course>
    {
         Task<int> CreateCourse(CourseRequest request);

        Task<List<CourseResponse>> GetAllCourses(HttpRequest request, int pageNumber = 1, int pageSize = 1, bool onlayActive = false);

        Task<int> UpdateFile(int id, CourseRequest request);

        Task<int> DeleteCourse(int id);

        Task<CourseResponse?> GetCourseByIdWithImages(int id, HttpRequest request); 
    }
}
