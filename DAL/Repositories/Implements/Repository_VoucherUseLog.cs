using DAL.Models;
using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implements
{
    public class RepositoryVoucherUseLog : Repositories_dal<VouchersUseLog>, IRepository_VoucherUseLog
    {
        public RepositoryVoucherUseLog(ShoeStoreContext _context) : base(_context)
        {
        }
        public override bool Delete(VouchersUseLog vouchersUseLog)
        {
            var result = 0;
            try
            {
                var model = _DbSet.FirstOrDefault(c => c.Id == vouchersUseLog.Id);
                if (model == null) return false;
                vouchersUseLog.Status = true;
                _DbSet.Update(vouchersUseLog);
                result = _Context.SaveChanges();
            }
            catch (Exception dbEx)
            {
            }
            return result > 0;
        }

        public VouchersUseLog GetVouchersUseLog(int id)
        {
            return _DbSet.FirstOrDefault(c => c.Id == id);
        }
    }
}
