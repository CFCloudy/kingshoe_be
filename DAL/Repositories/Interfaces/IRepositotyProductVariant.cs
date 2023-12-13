using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IRepositotyProductVariant : IRepository<ShoesVariant>
    {
        IEnumerable<ShoesVariant> GetAllShoeVariant();
        ShoesVariant GetShoesVariant(int shoesId);
    }
}
