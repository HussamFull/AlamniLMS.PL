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
    public class BrandRepository : GenericRepository<Brand> , IBrandRepository
    {
        private readonly ApplicationDbContext _context;

        public BrandRepository(ApplicationDbContext context) :base(context)
        {
            _context = context;
        }

        public async Task<Brand> GetAsync(int id)
        {
            return await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
