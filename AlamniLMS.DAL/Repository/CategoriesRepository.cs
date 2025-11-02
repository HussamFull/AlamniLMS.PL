using AlamniLMS.DAL.Data;
using AlamniLMS.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.DAL.Repository
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoriesRepository(ApplicationDbContext context) 
        {
            _context = context;
        }

        public int Add(Categories category)
        {
            _context.Categories.Add(category);
            return _context.SaveChanges();
        }

       

        public IEnumerable<Categories> GetAll(bool withTracking = false)
        {
            if (withTracking)
            return _context.Categories.ToList();

            return _context.Categories.AsNoTracking().ToList();
        }

        public Categories? GetById(int id)=>_context.Categories.Find(id);

        public int Remove(Categories category)
        {
            _context.Categories.Remove(category);
            return _context.SaveChanges();
        }

        public int Update(Categories category)
        {
            _context.Categories.Update(category);
            return _context.SaveChanges();
        }
    }
}
