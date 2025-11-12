using AlamniLMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.DAL.Repository.Interfaces
{
    public interface IBrandRepository : IGenericRepository<Brand>
    {

        Task<Brand> GetAsync(int id);
    }
}
