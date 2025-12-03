using AlamniLMS.BLL.Services.Interfacese;
using AlamniLMS.DAL.DTO.Requests;
using AlamniLMS.DAL.DTO.Responses;
using AlamniLMS.DAL.Models;
using AlamniLMS.DAL.Repository.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.BLL.Services.Classes
{
    public class LectureService : GenericService<LectureRequest, LectureResponse, Lecture>, ILectureService
    {
        private readonly ILectureRepository _repository;
        private readonly IFileService _fileService;

        public LectureService(ILectureRepository repository, IFileService fileService) : base(repository)
        {
            _repository = repository;
            _fileService = fileService;


        }

        public async Task<int> CreateFile(LectureRequest request)
        {
            var entity = request.Adapt<Lecture>();
            entity.CreatedAt = DateTime.UtcNow;

            // مثال: لو كان هناك حقل لرفع ملف فيديو في CourseRequest
            
            if (request.VideoUrl != null)
            {
                // 🚀 تحديد مسار الحفظ للملف كـ "videos"
                var videoPath = await _fileService.UploadAsync(request.VideoUrl, "videos");
                entity.VideoUrl = videoPath;
            }
            

            //if (request.VideoUrl != null)
            //{
            //    var imagePath = await _fileService.UploadAsync(request.VideoUrl);
            //    entity.VideoUrl = imagePath;
            //}
            //if (request.SubImages != null)
            //{
            //    var SubImagesPaths = await _fileService.UploadManyAsync(request.SubImages);
            //    entity.SubImages = SubImagesPaths.Select(img => new ProductImage { ImageName = img }).ToList();
            //}
            return _repository.Add(entity);
        }



        // 1 2 3  GetLectureById
        // **✅ التعديل هنا:** استقبال HttpRequest
        public LectureResponse GetLectureById(int id, HttpRequest request)
        {
            var lecture = _repository.GetById(id);
            if (lecture == null)
                return null;

            // 1. بناء المسار الأساسي
            var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";

            // 2. التحويل إلى DTO
            var response = lecture.Adapt<LectureResponse>();

            // 3. بناء رابط الفيديو العام
            if (!string.IsNullOrEmpty(lecture.VideoUrl))
            {
                // VideoUrlUrl هو الخاصية الجديدة التي أضفناها في DTO
                response.VideoUrlUrl = $"{baseUrl}/videos/{lecture.VideoUrl}".Replace("//", "/");
            }

            return response;
        }

        // **✅ الإضافة هنا:** دالة جلب الجميع الجديدة
        public IEnumerable<LectureResponse> GetAllLectures(HttpRequest request, bool onlyActive = false)
        {
            var entities = _repository.GetAll();
            if (onlyActive)
            {
                entities = entities.Where(e => e.Status == Status.Active);
            }

            // بناء المسار الأساسي
            var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";

            // التحويل وبناء الروابط
            return entities.Select(lecture =>
            {
                var response = lecture.Adapt<LectureResponse>();
                if (!string.IsNullOrEmpty(lecture.VideoUrl))
                {
                    response.VideoUrlUrl = $"{baseUrl}/videos/{lecture.VideoUrl}".Replace("//", "/");
                }
                return response;
            }).ToList();
        }





        //public LectureResponse GetLectureById(int id, HttpRequest request)
        //{
        //    var lecture = _repository.GetById(id);
        //    if (lecture == null)
        //        return null;

        //    // 1. بناء المسار الأساسي
        //    var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";


        //    return lecture.Adapt<LectureResponse>();
        //}
        // 1 2 3 UpdateLecture

        public async Task<int> UpdateLecture(int id, LectureRequest request)
        {
            var lecture = _repository.GetById(id);
            if (lecture == null)
                return 0;

            // تعديل البيانات
            lecture.Title = request.Title;
            lecture.Description = request.Description;
            lecture.Order = request.Order;
            lecture.DurationSeconds = request.DurationSeconds;

            // تعديل الفيديو
            if (request.VideoUrl != null)
            {
                // حذف القديم من السيرفر
                if (!string.IsNullOrEmpty(lecture.VideoUrl))
                {
                    _fileService.Delete(lecture.VideoUrl);
                }

                // رفع الجديد
                var newVideo = await _fileService.UploadAsync(request.VideoUrl, "videos");
                lecture.VideoUrl = newVideo;
            }

            lecture.UpdatedAt = DateTime.UtcNow;

            return _repository.Update(lecture);
        }


        // 1 2 3 DeleteLecture
        public int DeleteLecture(int id)
        {
            var lecture = _repository.GetById(id);
            if (lecture == null)
                return 0;

            // حذف الفيديو من السيرفر
            if (!string.IsNullOrEmpty(lecture.VideoUrl))
            {
                _fileService.Delete(lecture.VideoUrl);
            }

            return _repository.Remove(lecture);
        }

    }
}
