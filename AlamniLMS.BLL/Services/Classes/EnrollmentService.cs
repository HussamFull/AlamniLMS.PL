using AlamniLMS.BLL.Services.Interfacese;
using AlamniLMS.DAL.DTO.Requests;
using AlamniLMS.DAL.DTO.Responses;
using AlamniLMS.DAL.Models;
using AlamniLMS.DAL.Repository.Classes;
using AlamniLMS.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.BLL.Services.Classes
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;

        public EnrollmentService(IEnrollmentRepository enrollmentRepository ) 
        {
            _enrollmentRepository = enrollmentRepository;
        }
        public async Task<bool> AddToEnrollmentAsync(EnrollmentRequest request, string UserId)
        {
            var newItem = new Enrollment
            {
                CourseId = request.CourseId,
                UserId = UserId,
                Count = 1
            }
         ;
            return await _enrollmentRepository.AddAsync(newItem) > 0;

        }

        public async Task<EnrollmentSummaryResponse> EnrollmentSummaryResponseAsync(string UserId)
        {
            var EnrollmenItems = await _enrollmentRepository.GetUserEnrollmentAsync(UserId);
            var response = new EnrollmentSummaryResponse
            {
                Items = EnrollmenItems.Select(ci => new EnrollmentResponse
                {
                    CourseId = ci.CourseId,
                    CourseTitle = ci.Course.Title,
                    Price = ci.Course.Price,
                    Count = ci.Count,
                    ThumbnailPath = ci.Course.ThumbnailPath,
                    CourseDescription = ci.Course.FullDescription
                }).ToList(),
            };
            return response;
        }

        public async Task<bool> ClearEnrollmentAsync(string UserId)
        {
            return await _enrollmentRepository.ClearEnrollmentAsync(UserId);
        }
    }
}
