using AlamniLMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.BLL.Services.Interfacese
{
    public interface IOrderService
    {
        Task<Order?> GetUserByOrderAsync(int orderId);

        Task<Order?> AddOrderAsync(Order order);

        // Task<List<Order>> GetAllWithUserAsync(string userId);

        Task<bool> ChangeStatusAsync(int orderId, OrderStatusEnum newStatus);

        Task<List<Order>> GetOrderByUserAsync(string userId);


        Task<List<Order>> GetByStatusAsync(OrderStatusEnum status);
    }
}
