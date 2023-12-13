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
    public class RepositoryProductVariant : RepositoryBase<ShoesVariant>, IRepositotyProductVariant
    {
        public RepositoryProductVariant(ShoeStoreContext context) : base(context)
        {

        }
        public override bool Delete(ShoesVariant shoesVariant)
        {
            var result = 0;
            try
            {
                var model = DbSet.AsNoTracking().FirstOrDefault(c => c.Id == shoesVariant.Id);
                if (model == null) return false;
                shoesVariant.Status = 1;
                DbSet.Update(shoesVariant);
                result = Context.SaveChanges();
            }
            catch (Exception dbEx)
            {
            }
            return result > 0;
        }

        public IEnumerable<ShoesVariant> GetAllShoeVariant()
        {
           var data = DbSet.AsNoTracking().Include(c => c.ColorNavigation).Include(c =>c.SizeNavigation).Include(c=>c.Product);
            return data;
        }

        public ShoesVariant GetShoesVariant(int shoesId)
        {
            return DbSet.AsNoTracking().Include(c => c.ColorNavigation).Include(c =>c.SizeNavigation).Include(c=>c.Product).FirstOrDefault(c => c.Id == shoesId);
        }
    }
}
