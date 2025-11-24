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
    public class CertificateRepository : GenericRepository<Certificate>, ICertificateRepository
    {
        private readonly ApplicationDbContext _context;

        public CertificateRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Certificate> GetByUserAndCourseAsync(string userId, int courseId)
        {
            return await _context.Certificates
                .Include(c => c.Course)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.CourseId == courseId);
        }
    }
}
