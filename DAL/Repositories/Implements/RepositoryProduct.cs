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
    public class RepositoryProduct : RepositoryBase<Shoe>, IRepositoryProduct
    {
        public RepositoryProduct(ShoeStoreContext context) : base(context)
        {

        }
        public override bool Delete(Shoe shoe)
        {
            var result = 0;
            try
            {
                var model = DbSet.AsNoTracking().FirstOrDefault(c => c.Id == shoe.Id);
                if (model == null) return false;
                shoe.Status = 1;
                DbSet.Update(shoe);
                result = Context.SaveChanges();
            }
            catch (Exception dbEx)
            {
            }
            return result > 0;
        }

        public IEnumerable<Shoe> GetAllShoe()
        {
            var data = DbSet.AsNoTracking().Include(c => c.BrandGroupNavigation).Include(c =>c.FeatureNavigation).Include(c=>c.StyleGroupNavigation);
            return data;
        }

        public Shoe GetShoeById(int shoeId)
        {
            return DbSet.AsNoTracking().Include(c => c.BrandGroupNavigation).Include(c =>c.FeatureNavigation).Include(c=>c.StyleGroupNavigation).FirstOrDefault(c => c.Id == shoeId);
        }
    }
}
