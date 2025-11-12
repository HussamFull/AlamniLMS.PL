using AlamniLMS.BLL.Services.Interfacese;
using AlamniLMS.DAL.DTO.Requests;
using AlamniLMS.DAL.DTO.Responses;
using AlamniLMS.DAL.Models;
using AlamniLMS.DAL.Repository.Interfaces;
using Mapster;
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

        
    }
}
