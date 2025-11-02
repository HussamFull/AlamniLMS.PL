using AlamniLMS.BLL.Services.Interfacese;
using AlamniLMS.DAL.DTO.Requests;
using AlamniLMS.DAL.DTO.Responses;
using AlamniLMS.DAL.Models;
using AlamniLMS.DAL.Repository;
using Mapster;
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
        public int CreateCategories(CategoriesRequest request)
        {
            var categoty = request.Adapt<Categories>();
            return _categoriesRepository.Add(categoty);
        }

        public int DeleteCategories(int id)
        {
            var category = _categoriesRepository.GetById(id);
            if (category == null)
            {
                throw new Exception("Category not found");
            }
            
            return _categoriesRepository.Remove(category);

        }

        public IEnumerable<CategoriesResponses> GetAllCategories()
        {
            var categories = _categoriesRepository.GetAll();
            return categories.Adapt<IEnumerable<CategoriesResponses>>();
        }

        public CategoriesResponses? GetCategoriesById(int id)
        {
            var category = _categoriesRepository.GetById(id);
            if (category == null)
            {
                throw new Exception("Category not found");
            }
            return category.Adapt<CategoriesResponses>();
        }

        public int UpdateCategories(int id, CategoriesRequest category)
        {
            var Category = _categoriesRepository.GetById(id);
            if (Category == null)
            {
                throw new Exception("Category not found");
            }
            Category.Name = category.Name;
            Category.Description = category.Description;


            return _categoriesRepository.Update(Category);
        }

        public bool ToggleStatus(int id)
        {
            var category = _categoriesRepository.GetById(id);
            if (category == null)
            {
                throw new Exception("Category not found");
            }
            category.Status = category.Status == Status.Active ? Status.Inactive : Status.Active;
            _categoriesRepository.Update(category);
            return true;
        }

    }
}
