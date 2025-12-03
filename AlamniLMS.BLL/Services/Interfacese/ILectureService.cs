using AlamniLMS.DAL.DTO.Requests;
using AlamniLMS.DAL.DTO.Responses;
using AlamniLMS.DAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.BLL.Services.Interfacese
{
    public interface ILectureService : IGenericService<LectureRequest, LectureResponse, Lecture>
    {
            Task<int> CreateFile(LectureRequest request);
        // 1 2 3
        // LectureResponse GetLectureById(int id);

        // 1 2 3 
        // **✅ التعديل هنا:** إضافة HttpRequest
        LectureResponse GetLectureById(int id, HttpRequest request);

        // **✅ الإضافة هنا:** دالة جلب الجميع
        IEnumerable<LectureResponse> GetAllLectures(HttpRequest request, bool onlyActive = false);
        Task<int> UpdateLecture(int id, LectureRequest request);
        int DeleteLecture(int id);
    }
}
