using AlamniLMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.DAL.Repository.Interfaces
{
    public interface IReviewRepository
    {

        Task<bool> HasUserReviewdProduct(string userId, int productId);

        Task AddReviewAsync(Review review, string userId);
    }

}
