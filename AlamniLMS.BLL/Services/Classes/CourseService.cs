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
    public class CourseService : GenericService<CourseRequest, CourseResponse, Course>, ICourseService
    {
        private readonly ICourseRepository _repository;
        private readonly IFileService _fileService;

        public CourseService(ICourseRepository repository, IFileService fileService) : base(repository)
        {
            _repository = repository;
            _fileService = fileService;


        }

        public async Task<int> CreateCourse(CourseRequest request)
        {
            var entity = request.Adapt<Course>();
            entity.CreatedAt = DateTime.UtcNow;

            if (request.ThumbnailPath != null)
            {
                var imagePath = await _fileService.UploadAsync(request.ThumbnailPath, "images");
                entity.ThumbnailPath = imagePath;
            }
            if (request.SubImages != null)
            {
                var SubImagesPaths = await _fileService.UploadManyAsync(request.SubImages, "SubImage");
                entity.SubImages = SubImagesPaths.Select(img => new CourseImage { ImageName = img }).ToList();
            }
            return _repository.Add(entity);
        }

        //public async Task<List<CourseResponse>> GetAllCourses(HttpRequest request, int pageNumber = 1, int pageSize = 1, bool onlayActive = false)
        public async Task<List<CourseResponse>> GetAllCourses(HttpRequest request, bool onlayActive = false)

        {
            var courses = _repository.GetAllCoursesWithImage();

            if (onlayActive)
            {
                courses = courses.Where(p => p.Status == Status.Active).ToList();
            }
            //var pagedProducts = courses.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return courses.Select(p => new CourseResponse
            {
                Id = p.Id,
                Title = p.Title,
                FullDescription = p.FullDescription,
                //Quantity = p.Quantity,
                ThumbnailPathUrl = $"{request.Scheme}://{request.Host}/Images/{p.ThumbnailPath}",
                SubImagesUrls = p.SubImages.Select(img => $"{request.Scheme}://{request.Host}/Images/SubImage/{img.ImageName}").ToList(),
                //Reviews = p.Reviews.Select(r => new ReviewResponse
                //{
                //    Id = r.Id,
                //    FullName = r.User.FullName,
                //    Comment = r.Comment,
                //    Rate = r.Rate
                //}).ToList()

            }).ToList();
        }

        
    }
}
