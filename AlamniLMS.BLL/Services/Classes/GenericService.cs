using AlamniLMS.BLL.Services.Interfacese;
using AlamniLMS.DAL.DTO.Responses;
using AlamniLMS.DAL.Models;
using AlamniLMS.DAL.Repository.Classes;
using AlamniLMS.DAL.Repository.Interfaces;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.BLL.Services.Classes
{
    public class GenericService<TRequest, TResponse, TEntity> : IGenericService<TRequest, TResponse, TEntity>
        where TEntity : BaseModel
    {
        private readonly IGenericRepository<TEntity> _repository;

        public GenericService(IGenericRepository<TEntity> genericRepository)
        {
            _repository = genericRepository;
        }

        public int Create(TRequest request)
        {
            var entity = request.Adapt<TEntity>();
            return _repository.Add(entity);
        }

        public int Delete(int id)
        {
            var entity = _repository.GetById(id);
            if (entity == null) return 0;


            return _repository.Remove(entity);
        }

        public IEnumerable<TResponse> GetAll(bool onlyActive = false)
        {
            var entity = _repository.GetAll();
            if (onlyActive)
            {
                entity = entity.Where(e => e.Status == Status.Active);
            }
            return entity.Adapt<IEnumerable<TResponse>>();
        }

        public TResponse? GetById(int id)
        {
            var entity = _repository.GetById(id);
           
            return entity is null ? default : entity.Adapt<TResponse>();
        }

        public bool ToggleStatus(int id)
        {
            var entity = _repository.GetById(id);
            if (entity == null) return false;

            entity.Status = entity.Status == Status.Active ? Status.Inactive : Status.Active;
            _repository.Update(entity);
            return true;
        }

        public int Update(int id, TRequest request)
        {
            var entity = _repository.GetById(id);
            if (entity == null)
            {
                throw new Exception("entity not found");
            }
         
           var updateEntity = request.Adapt(entity);


            return _repository.Update(updateEntity);
        }
    }
}
