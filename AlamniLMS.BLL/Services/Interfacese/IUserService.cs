using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlamniLMS.DAL.DTO.Responses;


namespace AlamniLMS.BLL.Services.Interfacese
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto> GetByIdAsync(string userId);

        Task<bool> BlockUserAsync(string userId, int days);
        Task<bool> IsBlockedAsync(string userId);
        Task<bool> UnBlockUserAsync(string userId);

        Task<bool> ChangeUserRoleAsync(string userId, string roleName);


    }
}
