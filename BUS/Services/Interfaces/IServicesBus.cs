using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.IServices
{
    public interface IServicesBus<T>
    {
        Task<List<T>> GetAll();
        Task<bool> Insert(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
        Task<T> Getone(int id);
    }
}
