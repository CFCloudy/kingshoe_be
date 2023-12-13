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
    public class RepositoryColor : RepositoryBase<Color>, IRepositoryColor
    {
        public RepositoryColor(ShoeStoreContext context) : base(context)
        {

        }
        public override bool Delete(Color color)
        {
            var result = 0;
            try
            {
                var model = DbSet.AsNoTracking().FirstOrDefault(c => c.Id == color.Id);
                if (model == null) return false;
                color.Status = true;
                DbSet.Update(color);
                result = Context.SaveChanges();
            }
            catch (Exception dbEx)
            {
            }
            return result > 0;
        }

        public Color GetColor(int id)
        {
            return DbSet.AsNoTracking().FirstOrDefault(c=>c.Id == id);
        }
    }
}
