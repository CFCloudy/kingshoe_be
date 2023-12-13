using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IRepositoryProduct : IRepository<Shoe>
    {
        IEnumerable<Shoe> GetAllShoe();
        Shoe GetShoeById(int shoeId);
    }
}
