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
    public class RepositorySize : RepositoryBase<Size>, IRepositorySize
    {
        public RepositorySize(ShoeStoreContext context) : base(context)
        {

        }
        public override bool Delete(Size size)
        {
            var result = 0;
            try
            {
                var model = DbSet.AsNoTracking().FirstOrDefault(c => c.Id == size.Id);
                if (model == null) return false;
                size.Status = true;
                DbSet.Update(size);
                result = Context.SaveChanges();
            }
            catch (Exception dbEx)
            {
            }
            return result > 0;
        }

        public Size GetSize(int id)
        {
            return DbSet.AsNoTracking().FirstOrDefault(c => c.Id == id);
        }
    }
}
