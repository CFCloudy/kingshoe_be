using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        public IQueryable<T> GetAllDataQuery();
        public void AddDataCommand(T entity);
        public int AddDataCommandReturnId(T entity);
        public void UpdateDataCommand(T entity);
        public void DeleteDataCommand(T entity);
        public void UpdateDataWithDetachCommand(T entity);
    }
}
