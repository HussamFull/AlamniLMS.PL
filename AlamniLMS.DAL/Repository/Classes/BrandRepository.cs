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

        public BrandRepository(ApplicationDbContext context) :base(context)
        {
     
        }

    }
}
