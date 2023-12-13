using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.IService
{
    public interface IRepositoryShoeVariant_BUS : IRepository_BUS<ShoesVariant>
    {
        Task<List<ShoesVariant>> GetShoesVariantsByIdShoe(int idShoe);
        Task<bool> UpdateImage(ShoesVariant entity);
    }
}
