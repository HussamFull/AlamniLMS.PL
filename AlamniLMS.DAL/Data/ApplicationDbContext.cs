using AlamniLMS.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}
