using AlamniLMS.DAL.DTO.Requests;
using AlamniLMS.DAL.DTO.Responses;
using AlamniLMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.BLL.Services.Interfacese
{
    public interface ICategoriesService
    {
        int CerateCategories(CategoriesRequest request);
        IEnumerable<CategoriesResponses> GetAllCategories();
        CategoriesResponses GetCategoriesById(int id);
        int UpdateCategories(int id, CategoriesRequest category);
        int DeleteCategories(int id);
    }
}
