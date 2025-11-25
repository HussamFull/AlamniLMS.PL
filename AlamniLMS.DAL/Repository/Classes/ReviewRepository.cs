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
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<bool> HasUserReviewdProduct(string userId, int courseId)
        {

            return await _context.Reviews.AnyAsync(r => r.UserId == userId && r.CourseId == courseId);
        }



        public async Task AddReviewAsync(Review request, string userId)
        {
            request.UserId = userId;
            request.ReviewDate = DateTime.UtcNow;
            await _context.Reviews.AddAsync(request);
            await _context.SaveChangesAsync();
        }


    }

}
