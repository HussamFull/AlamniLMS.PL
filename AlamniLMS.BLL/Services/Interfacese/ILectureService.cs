using AlamniLMS.DAL.DTO.Requests;
using AlamniLMS.DAL.DTO.Responses;
using AlamniLMS.DAL.Models;
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
        LectureResponse GetLectureById(int id);
        Task<int> UpdateLecture(int id, LectureRequest request);
        int DeleteLecture(int id);
    }
}
