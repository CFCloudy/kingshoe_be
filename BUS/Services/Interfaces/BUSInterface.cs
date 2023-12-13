using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Repositories.Implements;
using DAL.Repositories.Interfaces;
using DTO.Simple;

namespace BUS.Services.Interfaces
{
    public interface BUSInterface<T> where T : class
    {
        public IQueryable<T> GetAll();
        public ReturnObject Update(T entity);
        public ReturnObject Delete(int id);
        public T GetById(int id);
        public ReturnObject Create(T entity);
    }

    public abstract class BaseService<T> : BUSInterface<T> where T : class
    {
        public abstract ReturnObject Create(T entity);
        public abstract ReturnObject Delete(int id);
        public abstract IQueryable<T> GetAll();
        public abstract T GetById(int id);
        public abstract ReturnObject Update(T entity);
        public abstract Tuple<List<T>,int> GetByRequest(string searchKey = "", int currentPage = 1, int rowPerPage = 20);
    }
}
