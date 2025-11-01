using AlamniLMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.DAL.Repository
{
    public interface ICategoriesRepository
    {
        int Add(Categories category);
        IEnumerable<Categories> GetAll();
        Categories GetById(int id);

        int Update(Categories category);
        int Delete(Categories category);
       
    }
}
