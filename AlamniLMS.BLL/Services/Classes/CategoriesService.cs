using AlamniLMS.BLL.Services.Interfacese;
using AlamniLMS.DAL.DTO.Requests;
using AlamniLMS.DAL.DTO.Responses;
using AlamniLMS.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.BLL.Services.Classes
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoriesRepository _categoriesRepository;

        public CategoriesService(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }
        public int CerateCategories(CategoriesRequest request)
        {
            throw new NotImplementedException();
        }

        public int DeleteCategories(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CategoriesResponses> GetAllCategories()
        {
            throw new NotImplementedException();
        }

        public CategoriesResponses GetCategoriesById(int id)
        {
            throw new NotImplementedException();
        }

        public int UpdateCategories(int id, CategoriesRequest category)
        {
            throw new NotImplementedException();
        }
    }
}
