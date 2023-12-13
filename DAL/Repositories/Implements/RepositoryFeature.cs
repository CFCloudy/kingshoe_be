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
    public class RepositoryFeature: RepositoryBase<Feature>, IRepositoryFeature
    {
         public RepositoryFeature(ShoeStoreContext context) : base(context)
        {

        }
        public override bool Delete(Feature feature)
        {
            var result = 0;
            try
            {
                var model = DbSet.AsNoTracking().FirstOrDefault(c => c.Id == feature.Id);
                if (model == null) return false;
                feature.Status = true;
                DbSet.Update(feature);
                result = Context.SaveChanges();
            }
            catch (Exception dbEx)
            {
            }
            return result > 0;
        }

        public Feature GetFeature(int id)
        {
            return DbSet.AsNoTracking().FirstOrDefault(c=>c.Id == id);
        }
    }
}
