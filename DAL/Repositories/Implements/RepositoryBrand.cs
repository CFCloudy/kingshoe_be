using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implements
{
    public class RepositoryBrand : RepositoryBase<Brand>, IRepositoryBrands
    {
        public RepositoryBrand(ShoeStoreContext context) : base(context)
        {

        }
        public override bool Delete(Brand brand)
        {
            var result = 0;
            try
            {
                var model = DbSet.AsNoTracking().FirstOrDefault(c => c.Id == brand.Id);
                if (model == null) return false;
                brand.Status = true;
                DbSet.Update(brand);
                result = Context.SaveChanges();
            }
            catch (Exception dbEx)
            {
            }
            return result > 0;
        }

        public Brand GetBrand(int id)
        {
            return DbSet.AsNoTracking().FirstOrDefault(c=>c.Id == id);
        }
    }
}
