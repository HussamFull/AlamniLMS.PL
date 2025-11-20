using AlamniLMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.DAL.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetUserByOrderAsync(int orderId);

        Task<Order?> AddAsync(Order order);

        //Task<List<Order>> GetAllWithUserAsync(string userId);

        //Task<bool> ChangeStatusAsync(int orderId, OrderStatusEnum newStatus);

        //Task<List<Order>> GetOrderByUserAsync(string userId);


        //Task<List<Order>> GetByStatusAsync(OrderStatusEnum status);

        //Task<bool> UserHasApprovedOrderForProductAsync(string userId, int productId);

    }
}
