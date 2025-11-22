using AlamniLMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.DAL.Repository.Interfaces
{
    public interface IEnrollmentRepository
    {
        Task<int> AddAsync(Enrollment enrollment);

        Task<List<Enrollment>> GetUserEnrollmentAsync(string UserId);

        Task<bool> ClearEnrollmentAsync(string UserId);
    }
}
