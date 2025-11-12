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
    public class CourseService : GenericService<CourseRequest, CourseResponse, Course>, ICourseService
    {
        private readonly ICourseRepository _repository;
        private readonly IFileService _fileService;

        public CourseService(ICourseRepository repository, IFileService fileService) : base(repository)
        {
            _repository = repository;
            _fileService = fileService;


        }

        public async Task<int> CreateFile(CourseRequest request)
        {
            var entity = request.Adapt<Course>();
            entity.CreatedAt = DateTime.UtcNow;

            if (request.ThumbnailPath != null)
            {
                var imagePath = await _fileService.UploadAsync(request.ThumbnailPath, "images");
                entity.ThumbnailPath = imagePath;
            }
            //if (request.SubImages != null)
            //{
            //    var SubImagesPaths = await _fileService.UploadManyAsync(request.SubImages);
            //    entity.SubImages = SubImagesPaths.Select(img => new ProductImage { ImageName = img }).ToList();
            //}
            return _repository.Add(entity);
        }

        
    }
}
