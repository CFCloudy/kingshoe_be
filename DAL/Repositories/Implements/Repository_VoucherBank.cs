using DAL.Models;
using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implements
{
    public class RepositoryVoucherBank : Repositories_dal<VoucherBank>, IRepository_VouchersBank
    {
        public RepositoryVoucherBank(ShoeStoreContext _context) : base(_context)
        {
        }
        public override bool Delete(VoucherBank voucherBank)
        {
            var result = 0;
            try
            {
                var model = _DbSet.FirstOrDefault(c => c.Id == voucherBank.Id);
                if (model == null) return false;
                voucherBank.Status = true;
                _DbSet.Update(voucherBank);
                result = _Context.SaveChanges();
            }
            catch (Exception dbEx)
            {
            }
            return result > 0;
        }

        public VoucherBank GetVoucherBank(int voucherId)
        {
            return _DbSet.FirstOrDefault(c => c.Id == voucherId);
        }
    }
}
