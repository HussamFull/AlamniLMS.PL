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
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentRepository(ApplicationDbContext context) 
        {
            _context = context;
        }
        public async Task<int> AddAsync(Enrollment enrollment)
        {
            await _context.Enrollments.AddAsync(enrollment);
            return await _context.SaveChangesAsync();
        }

       

     

        public async Task<List<Enrollment>> GetUserEnrollmentAsync(string UserId)
        {
            // استخدام المنطق المشابه لدالة GetUserEnrollment ولكن باستخدام ToListAsync
            return await _context.Enrollments
                .Include(c => c.Course)
                .Where(c => c.UserId == UserId)
                .ToListAsync();
        }

        public async Task<bool> ClearEnrollmentAsync(string UserId)
        {
            var items = _context.Enrollments.Where(e => e.UserId == UserId).ToList();
            _context.Enrollments.RemoveRange(items);

             await _context.SaveChangesAsync();
            return true;
        }


    }
}
