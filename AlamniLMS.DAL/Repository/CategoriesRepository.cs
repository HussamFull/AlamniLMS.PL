using AlamniLMS.DAL.Data;
using AlamniLMS.DAL.Models;
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
            throw new NotImplementedException();
        }

        public int Delete(Categories category)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Categories> GetAll()
        {
            throw new NotImplementedException();
        }

        public Categories GetById(int id)
        {
            throw new NotImplementedException();
        }

        public int Update(Categories category)
        {
            throw new NotImplementedException();
        }
    }
}
