using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.IService
{
    public interface IRepository_BUS<T>
    {
        Task<List<T>> GetAll();
        Task<bool> Insert(T entity);
        Task<int> InsertReturnId(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
        Task<T> Getone(int id);
    }
}
