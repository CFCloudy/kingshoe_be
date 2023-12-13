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
    public class RepositoryStyle : RepositoryBase<Style>, IRepositoryStyle
    {
        public RepositoryStyle(ShoeStoreContext context) : base(context)
        {

        }
        public override bool Delete(Style style)
        {
            var result = 0;
            try
            {
                var model = DbSet.AsNoTracking().FirstOrDefault(c => c.Id == style.Id);
                if (model == null) return false;
                style.Status = true;
                DbSet.Update(style);
                result = Context.SaveChanges();
            }
            catch (Exception dbEx)
            {
            }
            return result > 0;
        }

        public Style GetStyle(int id)
        {
            return DbSet.AsNoTracking().FirstOrDefault(c => c.Id == id);
        }
    }
}
