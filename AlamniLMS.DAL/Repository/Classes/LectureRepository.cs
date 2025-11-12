using AlamniLMS.DAL.Data;
using AlamniLMS.DAL.Models;
using AlamniLMS.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.DAL.Repository.Classes
{
    public class LectureRepository : GenericRepository<Lecture> , ILectureRepository
    {
        public LectureRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
